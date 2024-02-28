import {useState} from 'react';
import GenreSelectModule from "../../modules/GenreSelectModule/GenreSelectModule.tsx";
import FilmsModule from "../../modules/FilmsModule/FilmsModule.tsx";
import PopularFilmsModule from "../../modules/PopularFilmsModule/PopularFilmsModule.tsx";

const FilmPage = () => {

    const [genre, genreSelect] = useState<string | undefined>()

    return (
        <>
            <GenreSelectModule genre={genre} className="mt-5" genreSelected={g => genreSelect(g)}/>
            <PopularFilmsModule className="mt-5"/>
            <FilmsModule className="mt-5" genre={genre}/>
        </>
    );
};

export default FilmPage;