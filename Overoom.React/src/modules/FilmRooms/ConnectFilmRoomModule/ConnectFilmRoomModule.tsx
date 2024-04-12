import {useInjection} from "inversify-react";
import {FilmRoomContextProvider} from "../../../contexts/FilmRoomContext/FilmRoomContext.tsx";
import {IProfileService} from "../../../services/ProfileService/IProfileService.ts";
import {IFilmRoomsService} from "../../../services/RoomsServices/FilmRoomsService/IFilmRoomsService.ts";
import {RoomServer} from "../../../services/RoomsServices/Common/ViewModels/RoomsViewModels.ts";
import {ReactNode, useCallback, useEffect, useState} from "react";
import {FilmRoom} from "../../../services/RoomsServices/FilmRoomsService/Models/FilmRoomsViewModels.ts";
import Loader from "../../../UI/Loader/Loader.tsx";
import Offcanvas from "../../../UI/Offcanvas/Offcanvas.tsx";
import ConnectRoomForm from "./ConnectRoomForm/ConnectRoomForm.tsx";
import {AxiosError} from "axios";

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
    const getFilmRoom = useCallback(async () => {
        const room = await roomsService.room(props.id)
        setFilmRoom(room)
    }, [props.id, roomsService]);

    // Подключение к комнате
    const connectToFilmRoom = useCallback(async (code?: string) => {
        try {
            const server = await roomsService.connect({code: code, id: props.id})
            setRoomServer(server)
        } catch (e) {
            if (e instanceof AxiosError) setConnectError(e.response?.data.errors.error)
        }
    }, [props.id, roomsService]);

    // Добавление комнаты в историю просмотров
    const addFilmRoomToHistory = useCallback(() => {
        if (!filmRoom || !roomServer) return;
        profileService.addToHistory(filmRoom.filmId).then();
    }, [filmRoom, roomServer, profileService]);

    useEffect(() => {
        getFilmRoom().then();
    }, [getFilmRoom]);

    useEffect(() => {
        if (!filmRoom) return;
        if (!filmRoom.isCodeNeeded) connectToFilmRoom().then();
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
                                     onChange={() => setConnectError(undefined)}/>
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