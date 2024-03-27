import {Col, Row} from "react-bootstrap";
import {PlaylistItemData} from "../PlaylistItem/PlaylistItemData.ts";
import PlaylistItem from "../PlaylistItem/PlaylistItem.tsx";
import InfiniteScroll from "react-infinite-scroll-component";
import Spinner from "../../Common/Spinner/Spinner.tsx";

interface PlaylistsProps {
    playlists: PlaylistItemData[],
    className?: string,
    genre?: string,
    onSelect: (playlist: PlaylistItemData) => void,
    hasMore: boolean,
    next: () => void
}

const PlaylistsCatalog = (props: PlaylistsProps) => {


    const scrollProps = {
        dataLength: props.playlists.length,
        next: props.next,
        hasMore: props.hasMore,
        loader: <Spinner/>,
        className: props.className
    }

    return (
        <InfiniteScroll {...scrollProps}>
            <Row className="gy-5 m-0 justify-content-start">
                {props.playlists.map(playlist =>
                    <Col sm={6} xxl={4} key={playlist.id}>
                        <PlaylistItem selectedGenre={props.genre} playlist={playlist}
                                      onClick={() => props.onSelect(playlist)}/>
                    </Col>
                )}
            </Row>
        </InfiniteScroll>
    );
};

export default PlaylistsCatalog;