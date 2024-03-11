import styles from "./FilmShortItem.module.css"
import {FilmShortData} from "./FilmShortData.ts";

const FilmShortItem = ({film, className = '', onClick}: {
    className?: string,
    film: FilmShortData,
    onClick: () => void
}) => {
    return (
        <div className={`${styles.blacker_blur} ${className}`.trim()} onClick={onClick}>
            <img alt="Постер" src={film.posterUrl} className={styles.poster}/>
            <div className={styles.black}></div>
            {film.ratingKp &&
                <small className={styles.kp}>KP: {film.ratingKp}</small>}
            {film.ratingImdb &&
                <small className={styles.imdb}>IMDB: {film.ratingImdb}</small>}

            <small className={styles.title}>{film.title}</small>
        </div>
    );
};

export default FilmShortItem;