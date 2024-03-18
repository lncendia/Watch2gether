import {useEffect, useRef} from "react";
import FilmRoomPlayer from "../../../components/FilmRoom/FilmRoomPlayer/FilmRoomPlayer.tsx";
import {IPlayerHandler} from "./IPlayerHandler.ts";
import {PlayerJsHandler} from "./PlayerJsHandler.ts";
import {useFilmRoom} from "../../../contexts/FilmRoomContext/FilmRoomContext.tsx";
import {useUser} from "../../../contexts/UserContext/UserContext.tsx";

const FilmRoomPlayerModule = () => {

    const frame = useRef<HTMLIFrameElement>(null)
    const {room, service, updatePause, updateTimeLine, changeSeries} = useFilmRoom()
    const {authorizedUser} = useUser()

    const changeSeriesRef = useRef(changeSeries)
    const updateTimeLineRef = useRef(updateTimeLine)
    const updatePauseRef = useRef(updatePause)

    useEffect(() => {
        updatePauseRef.current = updatePause
        updateTimeLineRef.current = updateTimeLine
        changeSeriesRef.current = changeSeries
    }, [updatePause, updateTimeLine, changeSeries]);

    useEffect(() => {

        let handler: IPlayerHandler

        if (room.cdnName === "VideoCDN") handler = new PlayerJsHandler(frame.current!)
        // else if (room.cdnName === "Kodik") handler = new KodikHandler(service)
        else throw new Error()

        handler.pause.attach(async (data: [boolean, number]) => {
            updatePauseRef.current(authorizedUser!.id, data[0], data[1])
            await service.setPause(data[0], data[1])
        })

        handler.seek.attach(async (second: number) => {
            updateTimeLineRef.current(authorizedUser!.id, second)
            await service.setTimeLine(second)
        })

        handler.changeSeries.attach(async ([season, series]) => {
            changeSeriesRef.current(authorizedUser!.id, season, series)
            await service.changeSeries(season, series)
        })

    }, [authorizedUser, room.cdnName, service]);

    return (
        <FilmRoomPlayer reference={frame} src={room.cdnUrl}/>
    );
};

export default FilmRoomPlayerModule;