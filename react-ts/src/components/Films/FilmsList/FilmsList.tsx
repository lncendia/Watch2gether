import FilmShortItem from "../FilmShortItem/FilmShortItem.tsx";
import {FilmShortData} from "../FilmShortItem/FilmShortData.ts";
import {Col, Row} from "react-bootstrap";
import ContentBlock from "../../../UI/ContentBlock/ContentBlock.tsx";

const FilmsList = ({films, onFilmSelect, className}: {
    films: FilmShortData[],
    className?: string,
    onFilmSelect: (film: FilmShortData) => void
}) => {

    return (
        <ContentBlock className={className}>
            <Row className="gy-4">
                {films.map(film =>
                    <Col xs={6} sm={4} lg={3} xxl={2} key={film.filmId}>
                        <FilmShortItem film={film} onClick={() => onFilmSelect(film)}/>
                    </Col>
                )}
            </Row>
        </ContentBlock>
    );
};

export default FilmsList;