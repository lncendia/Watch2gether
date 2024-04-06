import styles from './FilmSearchElement.module.css'
import {FilmShort} from "../../../services/FilmsService/Models/Films.ts";

const FilmSearchElement = ({film, onClick}: { film: FilmShort, onClick: () => void }) => {
    return (
        <div className={styles.element} onClick={onClick}>
            <img className={styles.poster} src={film.posterUrl} alt="Постер"/>
            <div className={styles.title}>{film.title}
                <div className={styles.description}>{film.description}</div>
            </div>
        </div>
    );
};

export default FilmSearchElement;