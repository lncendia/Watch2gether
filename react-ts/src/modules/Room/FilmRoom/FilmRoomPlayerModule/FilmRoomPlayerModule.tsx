import {useEffect, useRef} from "react";
import FilmRoomPlayer from "../../../../components/FilmRoom/FilmRoomPlayer/FilmRoomPlayer.tsx";
import {IPlayerHandler} from "./IPlayerHandler.ts";
import {PlayerJsHandler} from "./PlayerJsHandler.ts";
import {useFilmRoom} from "../../../../contexts/FilmRoomContext/FilmRoomContext.tsx";

const FilmRoomPlayerModule = () => {

    const frame = useRef<HTMLIFrameElement>(null)
    const {room, service, viewers, updatePause, updateTimeLine, changeSeries} = useFilmRoom()

    const changeSeriesRef = useRef(changeSeries)
    const updateTimeLineRef = useRef(updateTimeLine)
    const updatePauseRef = useRef(updatePause)
    const viewersRef = useRef(viewers)

    useEffect(() => {
        const intervalId = setInterval(() => {
            viewersRef.current.forEach(v => {
                if (!v.pause) {
                    updateTimeLineRef.current(v.id, v.second + 1)
                }
            })
        }, 1000)

        return () => clearInterval(intervalId)
    }, []);

    useEffect(() => {
        viewersRef.current = viewers
        updatePauseRef.current = updatePause
        updateTimeLineRef.current = updateTimeLine
        changeSeriesRef.current = changeSeries
    }, [updatePause, updateTimeLine, changeSeries, viewers]);

    useEffect(() => {

        let handler: IPlayerHandler

        if (room.cdnName === "VideoCDN") handler = new PlayerJsHandler(frame.current!)
        // else if (room.cdnName === "Kodik") handler = new KodikHandler(service)
        else throw new Error()

        handler.pause.attach(async (data: [boolean, number]) => {
            updatePauseRef.current(room.currentId, data[0], data[1])
            await service.setPause(data[0], data[1])
        })

        handler.seek.attach(async (second: number) => {
            updateTimeLineRef.current(room.currentId, second)
            await service.setTimeLine(second)
        })

        handler.changeSeries.attach(async ([season, series]) => {
            changeSeriesRef.current(room.currentId, season, series)
            await service.changeSeries(season, series)
        })

        service.pauseEvent.attach((event) => {
            if (event.userId === room.ownerId) {
                handler.setPause(event.onPause)
                handler.setSecond(event.seconds)
            }
            updatePauseRef.current(event.userId, event.onPause, event.seconds)
        })

        service.seekEvent.attach((event) => {
            if (event.userId === room.ownerId) {
                handler.setSecond(event.seconds)
            }
            updateTimeLineRef.current(event.userId, event.seconds)
        })

        return handler.unmount.bind(handler)

    }, [room.cdnName, room.currentId, room.ownerId, service]);

    return (
        <FilmRoomPlayer reference={frame} src={room.cdnUrl}/>
    );
};

export default FilmRoomPlayerModule;