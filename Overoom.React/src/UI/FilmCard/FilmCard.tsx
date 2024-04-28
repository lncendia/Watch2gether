import React, {ReactNode} from 'react';
import styles from "./FilmCard.module.css";
import {Card, Col, Row} from "react-bootstrap";

interface FilmCardPosterProps {
    posterUrl: string,
    ratingKp?: number,
    ratingImdb?: number,
    header: string,
    className?: string,
    children: ReactNode,
    onClick: () => void
}

const FilmCard = (props: FilmCardPosterProps) => {
    return (
        <Card className={styles.film} onClick={props.onClick}>
            <Card.Header>
                <div className="float-start">{props.header}</div>
            </Card.Header>
            <Row className="g-0 h-100">
                <Col lg={4}>
                    <div className={styles.blacker_blur}>
                        <img alt="Постер" src={props.posterUrl} className={styles.poster}/>
                        <div className={styles.black}></div>
                        {props.ratingKp &&
                            <small className={styles.kp}>KP: {props.ratingKp}</small>}
                        {props.ratingImdb &&
                            <small className={styles.imdb}>IMDB: {props.ratingImdb}</small>}
                    </div>
                </Col>
                <Col lg={8}>
                    <Card.Body>
                        {props.children}
                    </Card.Body>
                </Col>
            </Row>
        </Card>
    );
};

export default FilmCard;