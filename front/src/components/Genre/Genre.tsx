import React from 'react';
import styles from "./Genre.module.css"

interface GenreProps {
    genre: string
    selected: boolean
    onSelect: () => void
}

const Genre = (props: GenreProps) => {
    return (
        <div className={`${styles.genre} ${props.selected ? styles.selected : ''}`} onClick={props.onSelect}>
            {props.genre}
        </div>
    );
};

export default Genre;