import {useState} from 'react';
import GenreSelectModule from "../../modules/Common/GenreSelectModule/GenreSelectModule.tsx";
import BlockTitle from "../../UI/BlockTitle/BlockTitle.tsx";
import PlaylistsModule from "../../modules/Playlists/PlaylistsModule/PlaylistsModule.tsx";

const PlaylistsPage = () => {

    const [genre, genreSelect] = useState<string | undefined>()

    return (
        <>
            <BlockTitle title="Жанры" className="mt-3 mb-3"/>
            <GenreSelectModule genre={genre} genreSelected={g => genreSelect(g)}/>
            <PlaylistsModule className="mt-5" genre={genre}/>
        </>
    );
};

export default PlaylistsPage;