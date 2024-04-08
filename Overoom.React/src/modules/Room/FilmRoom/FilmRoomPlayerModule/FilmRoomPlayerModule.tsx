import {useCallback, useEffect, useMemo, useRef} from "react";
import FilmRoomPlayer from "../../../../components/Room/FilmRoom/FilmRoomPlayer/FilmRoomPlayer.tsx";
import {IPlayerHandler} from "./IPlayerHandler.ts";
import {PlayerJsHandler} from "./PlayerJsHandler.ts";
import {useFilmRoom} from "../../../../contexts/FilmRoomContext/FilmRoomContext.tsx";
import {ChangeSeriesEvent, PauseEvent, SeekEvent} from "../../../../services/FilmRoomManager/ViewModels/FilmRoomEvents.ts";

const FilmRoomPlayerModule = ({className}: { className?: string }) => {

    const {room, service, viewersParams, updatePause, updateTimeLine, changeSeries} = useFilmRoom()
    const frame = useRef<HTMLIFrameElement>(null)

    const params = viewersParams.filter(v => v.id === room.currentId)[0]
    const currentData = useRef({
        season: params.season,
        episode: params.series,
        second: params.second
    })

    const handler = useMemo<IPlayerHandler>(() => {
        if (room.cdnName === "VideoCDN") return new PlayerJsHandler()
        // else if (room.cdnName === "Kodik") return new KodikHandler()
        else throw new Error()
    }, [room.cdnName]);


    const url = useMemo(() => {
        return handler.generateUrl(room.cdnUrl, currentData.current.second, currentData.current.season, currentData.current.episode)
    }, [handler, room.cdnUrl]);


    const handlePause = useCallback(async (data: [boolean, number]) => {
        updatePause(room.currentId, data[0], data[1])
        await service.setPause(data[0], data[1])
    }, [room.currentId, service, updatePause]);

    const handleSeek = useCallback(async (second: number) => {
        updateTimeLine(room.currentId, second)
        await service.setTimeLine(second)
    }, [room.currentId, service, updateTimeLine]);

    const handleChangeSeries = useCallback(async (data: [number, number]) => {
        changeSeries(room.currentId, data[0], data[1])
        await service.changeSeries(data[0], data[1])
    }, [room.currentId, service, changeSeries]);


    const handlePauseEvent = useCallback((event: PauseEvent) => {
        if (event.userId === room.ownerId) {
            handler.setPause(event.onPause)
            handler.setSecond(event.seconds)
        }
    }, [room.ownerId, handler]);

    const handleSeekEvent = useCallback((event: SeekEvent) => {
        if (event.userId === room.ownerId) {
            handler.setSecond(event.seconds)
        }
    }, [room.ownerId, handler]);


    const handleChangeSeriesEvent = useCallback((event: ChangeSeriesEvent) => {
        if (event.userId === room.ownerId) {
            handler.setSeries(event.season, event.series)
        }
    }, [room.ownerId, handler]);

    useEffect(() => {

        handler.mount(frame.current!)

        handler.pause.attach(handlePause)

        handler.seek.attach(handleSeek)

        handler.changeSeries.attach(handleChangeSeries)

        service.pauseEvent.attach(handlePauseEvent)

        service.seekEvent.attach(handleSeekEvent)

        service.changeSeriesEvent.attach(handleChangeSeriesEvent)

        return handler.unmount.bind(handler)

    }, [service, handler, handlePause, handleSeek, handleChangeSeries, handlePauseEvent, handleSeekEvent]);

    return <FilmRoomPlayer className={className} reference={frame} src={url}/>
};

export default FilmRoomPlayerModule;