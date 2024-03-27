import styles from "./FilmPoster.module.css"

interface FilmPosterProps {
    posterUrl: string,
    ratingKp?: number,
    ratingImdb?: number
    className?: string
}

const FilmPoster = ({posterUrl, ratingImdb, ratingKp, className = ''}: FilmPosterProps) => {
    return (
        <div className={`${styles.blacker_blur} ${className}`.trim()}>
            <img className={styles.poster} src={posterUrl} alt="Постер"/>
            <div className={styles.black}></div>
            {ratingKp &&
                <small className={styles.kp}>KP: {ratingKp}</small>}
            {ratingImdb &&
                <small className={styles.imdb}>IMDB: {ratingImdb}</small>}
        </div>
    );
};

export default FilmPoster;