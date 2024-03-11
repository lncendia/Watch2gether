import {Col, Row} from "react-bootstrap";
import {PlaylistItemData} from "../PlaylistItem/PlaylistItemData.ts";
import PlaylistItem from "../PlaylistItem/PlaylistItem.tsx";

export interface PlaylistsProps {
    playlists: PlaylistItemData[],
    className?: string,
    genre?: string,
    onPlaylistSelect: (playlist: PlaylistItemData) => void
}

const PlaylistsCatalog = ({playlists, genre, className = '', onPlaylistSelect}: PlaylistsProps) => {
    return (
        <Row className={`gy-5 m-0 justify-content-start ${className}`.trim()}>
            {playlists.map(playlist =>
                <Col sm={6} xxl={4} key={playlist.id}>
                    <PlaylistItem selectedGenre={genre} playlist={playlist} onClick={() => onPlaylistSelect(playlist)}/>
                </Col>
            )}
        </Row>
    );
};

export default PlaylistsCatalog;