import {Col, Row} from "react-bootstrap";
import FilmItem from "../FilmItem/FilmItem.tsx";
import {FilmItemData} from "../FilmItem/FilmItemData.ts";
import InfiniteScroll from "react-infinite-scroll-component";
import Spinner from "../../../UI/Spinner/Spinner.tsx";

export interface FilmsCatalogProps {
    films: FilmItemData[],
    className?: string,
    genre?: string,
    typeSelected: boolean
    onSelect: (film: FilmItemData) => void,
    hasMore: boolean,
    next: () => void
}

const FilmsCatalog = (props: FilmsCatalogProps) => {

    const scrollProps = {
        dataLength: props.films.length,
        next: props.next,
        hasMore: props.hasMore,
        loader: <Spinner/>,
        className: props.className
    }

    return (
        <InfiniteScroll {...scrollProps}>
            <Row className="gy-5 m-0 justify-content-start">
                {props.films.map(film =>
                    <Col sm={6} xxl={4} key={film.id}>
                        <FilmItem selectedGenre={props.genre} film={film} onClick={() => props.onSelect(film)}
                                  typeSelected={props.typeSelected}/>
                    </Col>
                )}
            </Row>
        </InfiniteScroll>
    );
};

export default FilmsCatalog;