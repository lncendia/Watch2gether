import styles from "./PopularFilmSliderItem.module.css"
import {FilmShort} from "../../services/FilmsService/Models/FilmShort.ts";

const PopularFilmSliderItem = ({film}: { film: FilmShort }) => {
    return (
        <div className={styles.film}>
            <div className={styles.film_inside}>
                <img className={styles.img} src={film.posterUrl} alt="Постер фильма"/>
                <div className={styles.title}>{film.title}</div>
            </div>
        </div>
    );
};

export default PopularFilmSliderItem;