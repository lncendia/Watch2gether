import {Col, Row} from "react-bootstrap";
import FilmItem from "../FilmItem/FilmItem.tsx";
import {FilmItemData} from "../FilmItem/FilmItemData.ts";

export interface FilmsCatalogProps {
    films: FilmItemData[],
    className?: string,
    genre?: string,
    typeSelected: boolean
    onFilmSelect: (film: FilmItemData) => void
}

const FilmsCatalog = ({films, genre, typeSelected, className = '', onFilmSelect}: FilmsCatalogProps) => {
    return (
        <Row className={`gy-5 m-0 justify-content-start ${className}`.trim()}>
            {films.map(film =>
                <Col sm={6} xxl={4} key={film.id}>
                    <FilmItem selectedGenre={genre} film={film} onClick={() => onFilmSelect(film)}
                              typeSelected={typeSelected}/>
                </Col>
            )}
        </Row>
    );
};

export default FilmsCatalog;