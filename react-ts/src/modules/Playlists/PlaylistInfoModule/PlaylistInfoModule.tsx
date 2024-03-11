import {useNavigate} from "react-router-dom";
import {useInjection} from "inversify-react";
import {useEffect, useState} from "react";
import PlaylistInfo from "../../../components/Playlists/PlaylistInfo/PlaylistInfo.tsx";
import {PlaylistInfoData} from "../../../components/Playlists/PlaylistInfo/PlaylistInfoData.ts";
import {IPlaylistsService} from "../../../services/PlaylistsService/IPlaylistsService.ts";
import Spinner from "../../../components/Common/Spinner/Spinner.tsx";

const PlaylistInfoModule = ({id, className}: { id: string, className?: string }) => {

    const [playlist, setPlaylist] = useState<PlaylistInfoData>()
    const playlistsService = useInjection<IPlaylistsService>('PlaylistsService');

    // Навигационный хук
    const navigate = useNavigate();

    useEffect(() => {
        const processPlaylist = async () => {
            const response = await playlistsService.get(id)
            setPlaylist(response)
        };

        processPlaylist().then()
    }, [id]);

    if (!playlist) return <Spinner/>

    return <PlaylistInfo className={className} playlist={playlist!}
                         onGenreSelect={(value) => navigate('/search', {state: {genre: value}})}/>
};

export default PlaylistInfoModule;