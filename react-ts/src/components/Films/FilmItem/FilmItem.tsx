import styles from "./FilmItem.module.css"
import {Card, Col, Row} from "react-bootstrap";
import {ReactNode} from "react";
import {FilmItemData} from "./FilmItemData.ts";

export interface FilmItemProps {
    film: FilmItemData,
    selectedGenre?: string,
    typeSelected: boolean,
    onClick: () => void
}

const FilmItem = ({film, selectedGenre, typeSelected, onClick}: FilmItemProps) => {

    const genres: ReactNode[] = [];

    for (let _i = 0; _i < film.genres.length; _i++) {
        let className = film.genres[_i] === selectedGenre ? styles.genre_active : styles.genre;
        let coma = _i !== film.genres.length - 1;
        genres.push(
            <span className={className} key={film.genres[_i]}>{film.genres[_i]}{coma && ", "}</span>
        )
    }

    return (
        <Card className={styles.film} onClick={onClick}>
            <Card.Header>
                <div className="float-start">Рейтинг: {film.userRating}</div>
            </Card.Header>
            <Row className="g-0">
                <Col lg={4}>
                    <div className={styles.blacker_blur}>
                        <img alt="Постер" src={film.posterUrl} className={styles.poster}/>
                        <div className={styles.black}></div>
                        {film.ratingKp &&
                            <small className={styles.kp}>KP: {film.ratingKp}</small>}
                        {film.ratingImdb &&
                            <small className={styles.imdb}>IMDB: {film.ratingImdb}</small>}
                    </div>
                </Col>
                <Col lg={8}>
                    <Card.Body>
                        <Card.Title>{film.title}</Card.Title>
                        <Card.Text className={`mb-3 ${styles.subtitle}`}>{film.description}</Card.Text>
                        <Card.Text>{genres}</Card.Text>
                        <small
                            className={typeSelected ? styles.type_active : styles.type}>{film.isSerial ? "Сериал" : "Фильм"}</small>
                    </Card.Body>
                </Col>
            </Row>
        </Card>
    );
}

export default FilmItem;