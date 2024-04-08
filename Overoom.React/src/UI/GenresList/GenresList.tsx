import {ReactNode} from 'react';
import styles from "./GenresList.module.css";

const GenresList = ({genres, selected}: { genres: string[], selected?: string }) => {

    const genresNodes: ReactNode[] = [];

    for (let _i = 0; _i < genres.length; _i++) {
        const className = genres[_i] === selected ? styles.genre_active : styles.genre;
        const coma = _i !== genres.length - 1;
        genresNodes.push(
            <span className={className} key={genres[_i]}>{genres[_i]}{coma && ", "}</span>
        )
    }

    return (
        <>
            {genresNodes.map(g => g)}
        </>
    );
};

export default GenresList;