import Spinner from "../../../UI/Spinner/Spinner.tsx";
import InfiniteScroll from "react-infinite-scroll-component";
import {Col, Row} from "react-bootstrap";
import FilmRoomItem from "../FilmRoomItem/FilmRoomItem.tsx";
import {FilmRoomItemData} from "../FilmRoomItem/FilmRoomItemData.ts";


export interface FilmRoomsCatalogProps {
    rooms: FilmRoomItemData[],
    className?: string,
    onSelect: (film: FilmRoomItemData) => void,
    hasMore: boolean,
    next: () => void
}

const FilmRoomsCatalog = (props: FilmRoomsCatalogProps) => {
    const scrollProps = {
        dataLength: props.rooms.length,
        next: props.next,
        hasMore: props.hasMore,
        loader: <Spinner/>,
        className: props.className
    }

    return (
        <InfiniteScroll {...scrollProps}>
            <Row className="gy-5 m-0 justify-content-start">
                {props.rooms.map(room =>
                    <Col sm={6} xxl={4} key={room.id}>
                        <FilmRoomItem room={room} onClick={() => props.onSelect(room)}/>
                    </Col>
                )}
            </Row>
        </InfiniteScroll>
    );
};

export default FilmRoomsCatalog;