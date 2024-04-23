import FilmsModule from "../../modules/Films/FilmsModule/FilmsModule.tsx";
import {useLocation} from "react-router-dom";
import PlaylistInfoModule from "../../modules/Playlists/PlaylistInfoModule/PlaylistInfoModule.tsx";

const PlaylistPage = () => {

    const {state} = useLocation();
    return (
        <>
            <PlaylistInfoModule id={state?.id} className="mt-3"/>
            <FilmsModule playlistId={state?.id}/>
        </>
    );
};

export default PlaylistPage;