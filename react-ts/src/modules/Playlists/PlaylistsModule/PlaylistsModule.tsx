import {useEffect, useState} from 'react';
import InfiniteScroll from "react-infinite-scroll-component";
import Spinner from "../../../components/Common/Spinner/Spinner.tsx";
import {useInjection} from "inversify-react";
import {IPlaylistsService} from "../../../services/PlaylistsService/IPlaylistsService.ts";
import {useNavigate} from "react-router-dom";
import PlaylistsCatalog from "../../../components/Playlists/PlaylistsCatalog/PlaylistsCatalog.tsx";
import {PlaylistItemData} from "../../../components/Playlists/PlaylistItem/PlaylistItemData.ts";

interface PlaylistsModuleProps {
    genre?: string;
    className?: string
}

const PlaylistsModule = (props: PlaylistsModuleProps) => {

    const [playlists, setPlaylists] = useState<PlaylistItemData[]>([]);
    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(false);
    const playlistsService = useInjection<IPlaylistsService>('PlaylistsService');

    // Навигационный хук
    const navigate = useNavigate();


    useEffect(() => {
        const processPlaylists = async () => {
            const response = await playlistsService.search({
                genre: props.genre
            })

            setPage(2);
            setHasMore(response.countPages > 1)
            setPlaylists(response.playlists)
        };

        processPlaylists().then()
    }, [props]); // Эффект будет вызываться при каждом изменении `genre`

    const onBottom = () => {
        const processPlaylists = async () => {
            const response = await playlistsService.search({
                genre: props.genre,
                page: page
            })
            setPage(page + 1);
            setHasMore(response.countPages !== page)
            setPlaylists([...playlists, ...response.playlists])
        };

        processPlaylists().then()
    }

    const onPlaylistSelect = (playlist: PlaylistItemData) => {
        navigate('/playlist', {state: {id: playlist.id}})
    }

    const scrollProps = {
        dataLength: playlists.length,
        next: onBottom,
        hasMore: hasMore,
        loader: <Spinner/>,
        className: props.className
    }

    return (
        <InfiniteScroll {...scrollProps}>
            <PlaylistsCatalog genre={props.genre} playlists={playlists} onPlaylistSelect={onPlaylistSelect}/>
        </InfiniteScroll>
    );
};

export default PlaylistsModule;