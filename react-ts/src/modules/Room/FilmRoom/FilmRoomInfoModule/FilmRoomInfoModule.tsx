import {useFilmRoom} from "../../../../contexts/FilmRoomContext/FilmRoomContext.tsx";
import ViewersList from "../../../../components/FilmRoom/ViewersList/ViewersList.tsx";
import {ViewerData} from "../../../../components/FilmRoom/Viewer/ViewerData.ts";
import {useCallback} from "react";

const map = (viewer: FilmViewerData): ViewerData => {

    const current = viewer.season && viewer.series ? `${viewer.season} сезон, ${viewer.series} серия` : undefined;

    return {
        ...viewer,
        ...viewer.allows,
        photoUrl: viewer.photoUrl ?? '/vite.svg',
        current: current
    }
}

const FilmRoomInfoModule = () => {

    const {viewers, room, service} = useFilmRoom()

    const onBeep = useCallback(async (viewer: ViewerData) => {
        await service.beep(viewer.id)
    }, [service])

    const onScream = useCallback(async (viewer: ViewerData) => {
        await service.scream(viewer.id)
    }, [service])

    return (
        <ViewersList viewers={viewers.map(map)} currentId={room.currentId} ownerId={room.ownerId} onBeep={onBeep}
                     onScream={onScream}/>
    );
};

export default FilmRoomInfoModule;