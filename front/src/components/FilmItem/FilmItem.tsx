import React from 'react';
import styles from "./FilmItem.module.css"
import {Card, Col, Row} from "react-bootstrap";
import Poster from "../../UI/Poster/Poster";
import CardLink from "../../UI/CardLink/CardLink";

export interface Film {
    readonly name: string;
    readonly description: string;
    readonly genres: string[];
    readonly type: string;
    readonly posterUrl: string;
    readonly id: string;
    readonly ratingKp: number;
    readonly ratingImdb: number;
    readonly ratingOveroom: number;
}

const FilmItem = ({film}: { film: Film }) => {
    return (
        <Card className={styles.film}>
            <Card.Header>
                <div className="float-start">Рейтинг: {film.ratingOveroom}</div>
            </Card.Header>
            <CardLink href="#film">
                <Row className="g-0">
                    <Col md={4}>
                        <Poster src={film.posterUrl}/>
                    </Col>
                    <Col md={8}>
                        <Card.Body>
                            <Card.Title>{film.name}</Card.Title>
                            <Card.Text className={`mb-2 ${styles.poster}`}>{film.description}</Card.Text>
                            <Card.Text>{film.genres.join(', ')}</Card.Text>
                            <Card.Text
                                className="position-absolute bottom-0 pb-2"><small>{film.type}</small></Card.Text>
                        </Card.Body>
                    </Col>
                </Row>
            </CardLink>
        </Card>
    );
}

export default FilmItem;