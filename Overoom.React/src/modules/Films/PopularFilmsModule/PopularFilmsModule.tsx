import {useCallback, useEffect, useState} from 'react';
import {useInjection} from "inversify-react";
import {IFilmsService} from "../../../services/FilmsService/IFilmsService.ts";
import {useNavigate} from "react-router-dom";
import {FilmShort} from "../../../services/FilmsService/Models/Films.ts";
import FilmsSlider from "../../../components/Films/FilmsSlider/FilmsSlider.tsx";
import {FilmShortData} from "../../../components/Films/FilmShortItem/FilmShortData.ts";

const map = (f: FilmShort): FilmShortData => {
    return {
        id: f.id,
        title: f.title,
        posterUrl: f.posterUrl,
        ratingKp: f.ratingKp,
        ratingImdb: f.ratingImdb,
        year: f.year,
    }
}

const PopularFilmsModule = ({className}: { className?: string }) => {

    const [films, setFilms] = useState<FilmShortData[]>([]);
    const filmService = useInjection<IFilmsService>('FilmsService');

    // Навигационный хук
    const navigate = useNavigate();


    useEffect(() => {
        const processFilms = async () => {
            const response = await filmService.popular()
            setFilms(response.map<FilmShortData>(map))
        };

        processFilms().then()
    }, []); // Эффект будет вызываться при каждом изменении `genre`

    const onSelect = useCallback((film: FilmShortData) => {
        navigate('/film', {state: {id: film.id}})
    }, [navigate])

    return <FilmsSlider className={className} films={films} onSelect={onSelect}/>
};

export default PopularFilmsModule;