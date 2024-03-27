import {useFilmRoom} from "../../../../contexts/FilmRoomContext/FilmRoomContext.tsx";
import {useCallback, useEffect, useMemo, useState} from "react";
import ChangeNameForm from "../../Common/ChangeNameForm/ChangeNameForm.tsx";
import {ViewerData} from "../../../../components/Room/Common/Viewer/ViewerData.ts";
import ChangeName from "../../../../components/Room/Common/ChangeName/ChangeName.tsx";
import ViewersList from "../../../../components/Room/Common/ViewersList/ViewersList.tsx";

const FilmRoomViewersModule = () => {

    const {
        viewers,
        viewersParams,
        room,
        service,
        updatePause,
        updateTimeLine,
        changeSeries,
        changeName,
        type,
        connectViewer,
        disconnectViewer,
        removeViewer
    } = useFilmRoom()

    const [viewerToChange, setViewerToChange] = useState<ViewerData>()

    const onBeep = useCallback(async (viewer: ViewerData) => {
        await service.beep(viewer.id)
    }, [service])

    const onScream = useCallback(async (viewer: ViewerData) => {
        await service.scream(viewer.id)
    }, [service])

    const onChange = useCallback((viewer: ViewerData) => {
        setViewerToChange(viewer)
    }, [])

    const onChangeHide = useCallback(() => {
        setViewerToChange(undefined)
    }, [])

    const onChangeSubmit = useCallback(async (username: string) => {
        if (!viewerToChange) return
        await service.changeName(viewerToChange.id, username)
            .then(() => setViewerToChange(undefined))
            .catch(() => setViewerToChange(undefined))
    }, [viewerToChange, service])


    useEffect(() => {
        service.pauseEvent.attach((event) => updatePause(event.userId, event.onPause, event.seconds))
        service.changeSeriesEvent.attach((event) => changeSeries(event.userId, event.season, event.series))
        service.seekEvent.attach((event) => updateTimeLine(event.userId, event.seconds))
        service.typeEvent.attach((id) => type(id))
        service.changeNameEvent.attach((event) => changeName(event.target, event.name))
        service.connectEvent.attach((event) => connectViewer(event))
        service.disconnectEvent.attach((id) => disconnectViewer(id))
        service.leaveEvent.attach((id) => removeViewer(id))

    }, [updatePause, updateTimeLine, type, changeSeries]);

    const mappedViewers = useMemo<ViewerData[]>(() => {
        return viewers.map<ViewerData>(v => {
            const params = viewersParams.filter(p => p.id === v.id)[0]
            const current = params.season && params.series ? `${params.season} сезон, ${params.series} серия` : undefined;
            return {
                id: v.id,
                beep: v.beep,
                scream: v.scream,
                change: v.change,
                username: v.username,
                photoUrl: v.photoUrl,
                current: current,
                fullScreen: params.fullScreen,
                online: params.online,
                pause: params.pause,
                second: params.second,
                typing: params.typing,
            }
        })
    }, [viewers, viewersParams])


    return (
        <>
            <ViewersList viewers={mappedViewers} currentId={room.currentId} ownerId={room.ownerId} onBeep={onBeep}
                         onScream={onScream} onChange={onChange}/>
            <ChangeName show={!!viewerToChange} onHide={onChangeHide}>
                <ChangeNameForm username={viewerToChange?.username} callback={onChangeSubmit}
                                self={viewerToChange?.id === room.currentId}/>
            </ChangeName>
        </>
    );
};

export default FilmRoomViewersModule;