import {useRef} from "react";
import FilmRoomPlayer from "../../../components/FilmRoom/FilmRoomPlayer/FilmRoomPlayer.tsx";
import {useInjection} from "inversify-react";
import {IFilmRoomService} from "../../../services/FilmRoomService/IFilmRoomService.ts";
import {IPlayerHandler} from "./IPlayerHandler.ts";
import {PlayerJsHandler} from "./PlayerJsHandler.ts";
import {KodikHandler} from "./KodikHandler.ts";

const FilmRoomPlayerModule = ({room}: { room: FilmRoom }) => {

    const frame = useRef<HTMLIFrameElement>(null)
    const roomService = useInjection<IFilmRoomService>('FilmRoomService');

    let handler: IPlayerHandler

    if (room.cdnName === "VideoCDN") handler = new PlayerJsHandler(roomService)
    else if (room.cdnName === "Kodik") handler = new KodikHandler(roomService)
    else throw new Error()

    window.addEventListener('message', handler.handler.bind(handler))

    return (
        <FilmRoomPlayer reference={frame} src={room.cdnUrl}/>
    );
};

export default FilmRoomPlayerModule;