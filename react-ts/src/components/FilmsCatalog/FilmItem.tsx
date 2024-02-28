import styles from "./FilmItem.module.css"
import {Card, Col, Row} from "react-bootstrap";
import {FilmShort} from "../../services/FilmsService/Models/FilmShort.ts";
import {ReactNode} from "react";

export interface FilmItemProps {
    film: FilmShort,
    selectedGenre?: string,
}

const FilmItem = ({film, selectedGenre}: FilmItemProps) => {

    const genres: ReactNode[] = [];

    for (let _i = 0; _i < film.genres.length; _i++) {
        let className = film.genres[_i] === selectedGenre ? styles.genre_active : styles.genre;
        let coma = _i !== film.genres.length - 1;
        genres.push(
            <span className={className} key={film.genres[_i]}>{film.genres[_i]}{coma && ", "}</span>
        )
    }

    return (
        <Card className={styles.film}>
            <Card.Header>
                <div className="float-start">Рейтинг: {film.userRating}</div>
            </Card.Header>
            <Row className="g-0">
                <Col md={4}>
                    <Card.Img alt="Постер" src={film.posterUrl} className={styles.poster} variant="top"/>
                </Col>
                <Col md={8}>
                    <Card.Body>
                        <Card.Title>{film.title}</Card.Title>
                        <Card.Text className={"mb-3"}>{film.description}</Card.Text>
                        <Card.Text>{genres}</Card.Text>
                        <Card.Text className="position-absolute bottom-0 pb-2">
                            <small>{film.isSerial ? "Сериал" : "Фильм"}</small>
                        </Card.Text>
                    </Card.Body>
                </Col>
            </Row>
        </Card>
    );
}

export default FilmItem;