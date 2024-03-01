import React from 'react';
import {FilmShort} from "../../services/FilmsService/Models/FilmShort.ts";
import styles from './FilmSearchElement.module.css'

const FilmSearchElement = ({film}: { film: FilmShort }) => {
    return (
        <div className={styles.element}>
            <img className={styles.poster} src={film.posterUrl} alt="Постер"/>
            <div className={styles.title}>{film.title}
                <div className={styles.description}>{film.description}</div>
            </div>
        </div>
    );
};

export default FilmSearchElement;