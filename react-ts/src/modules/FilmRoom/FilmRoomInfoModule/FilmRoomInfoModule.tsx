import {useFilmRoom} from "../../../contexts/FilmRoomContext/FilmRoomContext.tsx";
import {useEffect, useRef} from "react";
import ViewersList from "../../../components/FilmRoom/ViewersList/ViewersList.tsx";
import {ViewerData} from "../../../components/FilmRoom/Viewer/ViewerData.ts";
import {useUser} from "../../../contexts/UserContext/UserContext.tsx";

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

    const {viewers, updateTimeLine, room} = useFilmRoom()
    const {authorizedUser} = useUser()
    const viewersRef = useRef(viewers)
    const updateTimeLineRef = useRef(updateTimeLine)

    useEffect(() => {
        viewersRef.current = viewers
        updateTimeLineRef.current = updateTimeLine
    }, [viewers, updateTimeLine]);

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

    return (
        <ViewersList viewers={viewers.map(map)} currentId={authorizedUser!.id} ownerId={room.ownerId}/>
    );
};

export default FilmRoomInfoModule;