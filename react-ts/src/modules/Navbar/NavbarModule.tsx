import Menu from "../../components/Menu/Menu.tsx";
import {useState} from "react";
import {FilmShort} from "../../services/FilmsService/Models/FilmShort.ts";
import {useInjection} from "inversify-react";
import {IFilmService} from "../../services/FilmsService/IFilmService.ts";

const MyNavbar = () => {

    const [films, setFilms] = useState<FilmShort[]>([]);
    const filmService = useInjection<IFilmService>('FilmService');

    const onFilmSearch = async (value) => {
        if (value === '') setFilms([])
        else {
            const filmsResponse = await filmService.search({query: value})
            setFilms(filmsResponse.films)
        }
    }

    return (
        <Menu films={films} onFilmSearch={onFilmSearch}/>
    );
};

export default MyNavbar;