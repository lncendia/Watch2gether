import React from 'react';
import {Col, Row} from "react-bootstrap";
import FilmItem, {Film} from "../FilmItem/FilmItem";

const FilmList = ({films, className = ''}: { films: Film[], className?: string }) => {
    return (
        <Row className={`gy-5 justify-content-start ${className}`.trim()}>
            {films.map(film =>
                <Col sm={6} xl={4} key={film.id}>
                    <FilmItem film={film}/>
                </Col>
            )}
        </Row>
    );
};

export default FilmList;