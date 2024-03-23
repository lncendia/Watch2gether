import React, {useState} from "react";
import styles from "./FilmSearch.module.css"
import FilmSearchElement from "../FilmSearchElement/FilmSearchElement.tsx";
import Form from "react-bootstrap/Form";
import {FilmShort} from "../../../services/FilmsService/Models/Films.ts";

const FilmSearch = ({onFilmSearch, films, onClick}: {
    films: FilmShort[],
    onFilmSearch: (value: string) => void,
    onClick: (film: FilmShort) => void
}) => {

    const [value, setValue] = useState('')
    const [timeoutId, setTimeoutId] = useState<NodeJS.Timeout>()
    const [showFilms, setShowFilms] = useState(false)

    const onInput = (e: React.ChangeEvent<HTMLInputElement>) => {
        setValue(e.target.value)
        setShowFilms(true)
        clearTimeout(timeoutId);
        const thread = setTimeout(async () => {
            onFilmSearch(e.target.value)
        }, 500);

        setTimeoutId(thread)
    }

    const onBlur = () => {
        setTimeout(() =>
            setShowFilms(false), 400
        )
    }

    return (
        <>
            <Form.Control className={styles.input} value={value} type="text" placeholder="Название" onInput={onInput}
                          onClick={() => setShowFilms(true)} onBlur={onBlur}/>
            {showFilms && films.length !== 0 && <div className={styles.search}>
                {films.map(f => <FilmSearchElement key={f.id} film={f} onClick={() => onClick(f)}/>)}
            </div>}
        </>
    );
};

export default FilmSearch;