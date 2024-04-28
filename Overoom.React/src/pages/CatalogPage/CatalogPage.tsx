import {useState} from 'react';
import GenreSelectModule from "../../modules/Common/GenreSelectModule/GenreSelectModule.tsx";
import FilmsModule from "../../modules/Films/FilmsModule/FilmsModule.tsx";
import PopularFilmsModule from "../../modules/Films/PopularFilmsModule/PopularFilmsModule.tsx";
import BlockTitle from "../../UI/BlockTitle/BlockTitle.tsx";

const CatalogPage = () => {

    const [genre, genreSelect] = useState<string | undefined>()

    return (
        <>
            <BlockTitle title="Популярное сейчас" className="mt-3 mb-3"/>
            <PopularFilmsModule/>
            <BlockTitle title="Жанры" className="mt-5 mb-3"/>
            <GenreSelectModule genre={genre} genreSelected={g => genreSelect(g)}/>
            <FilmsModule className="mt-5" genre={genre}/>
        </>
    );
};

export default CatalogPage;