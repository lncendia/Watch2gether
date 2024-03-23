import {useInjection} from "inversify-react";
import {IFilmRoomsService} from "../../../../services/RoomsService/IFilmRoomsService.ts";
import Offcanvas from "../../../../components/Common/Offcanvas/Offcanvas.tsx";
import ConnectRoomForm from "../../../FilmRooms/ConnectRoomForm/ConnectRoomForm.tsx";
import {ReactNode, useCallback, useEffect, useState} from "react";
import {FilmRoom} from "../../../../services/RoomsService/Models/Rooms.ts";
import Loader from "../../../../UI/Loader/Loader.tsx";
import {RoomServer} from "../../../../services/RoomsService/Models/RoomServer.ts";
import {FilmRoomContextProvider} from "../../../../contexts/FilmRoomContext/FilmRoomContext.tsx";

interface ConnectFilmRoomModuleProps {
    id: string,
    code?: string,
    children: ReactNode
}

const ConnectFilmRoomModule = (props: ConnectFilmRoomModuleProps) => {

    const roomsService = useInjection<IFilmRoomsService>('FilmRoomsService');
    const [roomInfo, setRoomInfo] = useState<FilmRoom>()
    const [server, setServer] = useState<RoomServer>()
    const [formOpen, setFormOpen] = useState(true)

    const connect = useCallback((code?: string) => {
        roomsService.connect({code: code, id: props.id}).then(r => setServer(r))
    }, [props.id, roomsService]);

    useEffect(() => {
        roomsService.room(props.id).then(r => setRoomInfo(r))
    }, [props.id, roomsService]);

    useEffect(() => {
        if (!roomInfo) return;
        if (!roomInfo.isCodeNeeded) connect()
    }, [connect, roomInfo]);

    if (!roomInfo) return <Loader/>


    if (!server && roomInfo.isCodeNeeded) {

        return (
            <>
                <Offcanvas title={`Подключение к комнате "${roomInfo.title}"`} show={formOpen}
                           onClose={() => setFormOpen(false)}>
                    <ConnectRoomForm callback={connect} code={props.code}/>
                </Offcanvas>
                <Loader/>
            </>
        );
    }

    if (!server) return <Loader/>

    return (
        <FilmRoomContextProvider filmRoom={roomInfo} server={server}>
            {props.children}
        </FilmRoomContextProvider>
    )
};

export default ConnectFilmRoomModule;