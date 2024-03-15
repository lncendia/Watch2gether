import FilmRoomPlayerModule from "../FilmRoomPlayerModule/FilmRoomPlayerModule.tsx";
import {useInjection} from "inversify-react";
import {IFilmRoomService} from "../../../services/FilmRoomService/IFilmRoomService.ts";
import {useEffect, useState} from "react";
import Loader from "../../../UI/Loader/Loader.tsx";

const FilmRoomModule = ({id, url}: { id: string, url: string }) => {

    const [room, setRoom] = useState<FilmRoom>()

    const roomService = useInjection<IFilmRoomService>('FilmRoomService');

    useEffect(() => {
        roomService.roomLoad.attach((room) => setRoom(room))
        roomService.connect(id, url).then()
    }, []);

    if(!room) return <Loader/>

    return (
        <FilmRoomPlayerModule room={room}/>
    );
};

export default FilmRoomModule;