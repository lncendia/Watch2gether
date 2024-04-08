import {useNavigate} from "react-router-dom";
import {useInjection} from "inversify-react";
import {useCallback, useEffect, useState} from "react";
import PlaylistInfo from "../../../components/Playlists/PlaylistInfo/PlaylistInfo.tsx";
import {IPlaylistsService} from "../../../services/PlaylistsService/IPlaylistsService.ts";
import Spinner from "../../../UI/Spinner/Spinner.tsx";
import {Playlist} from "../../../services/PlaylistsService/ViewModels/PlaylistViewModels.ts";

const PlaylistInfoModule = ({id, className}: { id: string, className?: string }) => {
    const [playlist, setPlaylist] = useState<Playlist>()
    const playlistsService = useInjection<IPlaylistsService>('PlaylistsService');

    // Навигационный хук
    const navigate = useNavigate();

    useEffect(() => {
        const processPlaylist = async () => {
            const response = await playlistsService.get(id)
            setPlaylist(response)
        };

        processPlaylist().then()
    }, [id, playlistsService]);

    const onGenreSelect = useCallback((value: string) => navigate('/filmSearch', {state: {genre: value}}), [navigate])

    if (!playlist) return <Spinner/>

    return <PlaylistInfo className={className} {...playlist} onGenreSelect={onGenreSelect}/>
};

export default PlaylistInfoModule;