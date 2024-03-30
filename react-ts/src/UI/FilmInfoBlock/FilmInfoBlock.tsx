import {ReactNode} from 'react';
import {Col, Row} from "react-bootstrap";
import styles from "./FilmInfoBlock.module.css";

interface FilmInfoBlockProps {
    posterUrl: string,
    ratingKp?: number,
    ratingImdb?: number,
    description: string
    posterClassName?: string
    children: ReactNode
}

const FilmInfoBlock = (props: FilmInfoBlockProps) => {
    return (
        <Row>
            <Col xl={3} lg={4} className="text-center mb-3">
                <div className={`${styles.blacker_blur} ${props.posterClassName ?? ''}`.trim()}>
                    <img className={styles.poster} src={props.posterUrl} alt="Постер"/>
                    <div className={styles.black}></div>
                    {props.ratingKp &&
                        <small className={styles.kp}>KP: {props.ratingKp}</small>}
                    {props.ratingImdb &&
                        <small className={styles.imdb}>IMDB: {props.ratingImdb}</small>}
                </div>
            </Col>
            <Col xl={9} lg={8}>
                {props.children}
            </Col>
        </Row>
    );
};

export default FilmInfoBlock;