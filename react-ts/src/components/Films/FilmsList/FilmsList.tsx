import FilmShortItem from "../FilmShortItem/FilmShortItem.tsx";
import {FilmShortData} from "../FilmShortItem/FilmShortData.ts";
import {Col, Row} from "react-bootstrap";
import ContentBlock from "../../../UI/ContentBlock/ContentBlock.tsx";
import InfiniteScroll from "react-infinite-scroll-component";
import Spinner from "../../../UI/Spinner/Spinner.tsx";

interface FilmsListProps {
    films: FilmShortData[],
    className?: string,
    onSelect: (film: FilmShortData) => void,
    hasMore: boolean,
    next: () => void
}

const FilmsList = (props: FilmsListProps) => {

    const scrollProps = {
        dataLength: props.films.length,
        next: props.next,
        hasMore: props.hasMore,
        loader: <Spinner/>,
        className: "mt-3"
    }

    return (
        <ContentBlock className={props.className}>
            <InfiniteScroll {...scrollProps}>
                <Row className="gy-4 gx-0">
                    {props.films.map(film =>
                        <Col xs={6} sm={4} lg={3} xxl={2} key={film.id}>
                            <FilmShortItem film={film} onClick={() => props.onSelect(film)}/>
                        </Col>
                    )}
                </Row>
            </InfiniteScroll>
        </ContentBlock>
    );
};

export default FilmsList;