import {useState} from "react";
import {FilmShort} from "../../services/FilmsService/Models/FilmShort.ts";
import styles from "./FilmSearch.module.css"
import FilmSearchElement from "./FilmSearchElement.tsx";
import Form from "react-bootstrap/Form";

const FilmSearch = ({onFilmSearch, films}: { films: FilmShort[], onFilmSearch: (string) => void }) => {

    const [value, setValue] = useState('')
    const [timeoutId, setTimeoutId] = useState<NodeJS.Timeout>()
    const [showFilms, setShowFilms] = useState('')

    const onInput = (e) => {
        setValue(e.target.value)
        setShowFilms(true)
        clearTimeout(timeoutId);
        let thread = setTimeout(async () => {
            onFilmSearch(e.target.value)
        }, 500);

        setTimeoutId(thread)
    }

    return (
        <>
            <Form.Control className={styles.input} value={value} type="text" placeholder="Название" onInput={onInput}
                   onClick={() => setShowFilms(true)}/>
            {showFilms && films.length !== 0 && <div className={styles.search} onBlur={() => setShowFilms(false)}>
                {films.map(f => <FilmSearchElement key={f.id} film={f}/>)}
            </div>}
        </>
    );
};

export default FilmSearch;