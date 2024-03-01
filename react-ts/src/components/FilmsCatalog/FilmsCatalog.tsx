import {Col, Row} from "react-bootstrap";
import FilmItem from "./FilmItem.tsx";
import {FilmShort} from "../../services/FilmsService/Models/FilmShort.ts";

export interface FilmListProps{
    films: FilmShort[],
    className?: string,
    genre?: string
}

const FilmsCatalog = ({films, genre, className = ''}: FilmListProps) => {
    return (
        <Row className={`gy-5 m-0 justify-content-start ${className}`.trim()}>
            {films.map(film =>
                <Col sm={6} xl={4} key={film.id}>
                    <FilmItem selectedGenre={genre} film={film}/>
                </Col>
            )}
        </Row>
    );
};

export default FilmsCatalog;