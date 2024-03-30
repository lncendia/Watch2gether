import {useInjection} from "inversify-react";
import {IFilmRoomsService} from "../../../../services/RoomsService/IFilmRoomsService.ts";
import Offcanvas from "../../../../UI/Offcanvas/Offcanvas.tsx";
import ConnectRoomForm from "../../Common/ConnectRoomForm/ConnectRoomForm.tsx";
import {ReactNode, useCallback, useEffect, useState} from "react";
import {FilmRoom} from "../../../../services/RoomsService/Models/Rooms.ts";
import Loader from "../../../../UI/Loader/Loader.tsx";
import {RoomServer} from "../../../../services/RoomsService/Models/RoomServer.ts";
import {FilmRoomContextProvider} from "../../../../contexts/FilmRoomContext/FilmRoomContext.tsx";
import {IProfileService} from "../../../../services/ProfileService/IProfileService.ts";

interface ConnectFilmRoomModuleProps {
    id: string,
    code?: string,
    children: ReactNode
}

const ConnectFilmRoomModule = (props: ConnectFilmRoomModuleProps) => {

    const roomsService = useInjection<IFilmRoomsService>('FilmRoomsService');
    const profileService = useInjection<IProfileService>('ProfileService');
    const [filmRoom, setFilmRoom] = useState<FilmRoom>();
    const [roomServer, setRoomServer] = useState<RoomServer>();
    const [connectFormOpen, setConnectFormOpen] = useState(true);
    const [connectError, setConnectError] = useState<string>();

    // Получение данных о комнате
    const getFilmRoom = useCallback(() => {
        roomsService.room(props.id)
            .then(r => setFilmRoom(r))
    }, [props.id, roomsService]);

    // Подключение к комнате
    const connectToFilmRoom = useCallback((code?: string) => {
        roomsService.connect({code: code, id: props.id})
            .then(r => setRoomServer(r))
            .catch(e => setConnectError(e.response.data.errors.error))
    }, [props.id, roomsService]);

    // Добавление комнаты в историю просмотров
    const addFilmRoomToHistory = useCallback(() => {
        if (!filmRoom || !roomServer) return;
        profileService.addToHistory(filmRoom.filmId).then();
    }, [filmRoom, roomServer, profileService]);

    useEffect(() => {
        getFilmRoom();
    }, [getFilmRoom]);

    useEffect(() => {
        if (!filmRoom) return;
        if (!filmRoom.isCodeNeeded) connectToFilmRoom();
    }, [connectToFilmRoom, filmRoom]);

    useEffect(() => {
        addFilmRoomToHistory();
    }, [addFilmRoomToHistory]);

    if (!filmRoom) return <Loader/>;


    if (!roomServer && filmRoom.isCodeNeeded) {

        return (
            <>
                <Offcanvas title={`Подключение к комнате "${filmRoom.title}"`} show={connectFormOpen}
                           onClose={() => setConnectFormOpen(false)}>
                    <ConnectRoomForm warning={connectError} callback={connectToFilmRoom} code={props.code}
                                     onChange={() => setError(undefined)}/>
                </Offcanvas>
                <Loader/>
            </>
        );
    }

    if (!roomServer) return <Loader/>

    return (
        <FilmRoomContextProvider filmRoom={filmRoom} server={roomServer}>
            {props.children}
        </FilmRoomContextProvider>
    )
};

export default ConnectFilmRoomModule;