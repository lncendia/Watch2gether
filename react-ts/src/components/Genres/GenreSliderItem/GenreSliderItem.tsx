import styles from "./GenreSliderItem.module.css"

interface GenreProps {
    genre: string
    selected: boolean
    onSelect: () => void
}

const GenreSlider = (props: GenreProps) => {
    return (
        <div className={`${props.selected ? styles.selected : ''} ${styles.genre}`} onClick={props.onSelect}>
            {props.genre}
        </div>
    );
};

export default GenreSlider;