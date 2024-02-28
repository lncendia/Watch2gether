import styles from "./GenreSliderItem.module.css"

interface GenreProps {
    genre: string
    selected: boolean
    onSelect: () => void
}

const GenreSlider = (props: GenreProps) => {
    return (
        <div className={styles.genre}>
            <div className={`${styles.genre_inside} ${props.selected ? styles.selected : ''}`} onClick={props.onSelect}>
                {props.genre}
            </div>
        </div>
    );
};

export default GenreSlider;