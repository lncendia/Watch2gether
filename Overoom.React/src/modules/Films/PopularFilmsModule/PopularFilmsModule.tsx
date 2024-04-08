import {useCallback, useEffect, useState} from 'react';
import {useInjection} from "inversify-react";
import {IFilmsService} from "../../../services/FilmsService/IFilmsService.ts";
import {useNavigate} from "react-router-dom";
import FilmsSlider from "../../../components/Films/FilmsSlider/FilmsSlider.tsx";
import {FilmShortData} from "../../../components/Films/FilmShortItem/FilmShortData.ts";

const PopularFilmsModule = ({className}: { className?: string }) => {

    const [films, setFilms] = useState<FilmShortData[]>([]);
    const filmService = useInjection<IFilmsService>('FilmsService');

    const navigate = useNavigate();

    useEffect(() => {
        const processFilms = async () => {
            const response = await filmService.popular()
            setFilms(response)
        };
        processFilms().then()
    }, [filmService]);

    const onSelect = useCallback((film: FilmShortData) => {
        navigate('/film', {state: {id: film.id}})
    }, [navigate])

    return <FilmsSlider className={className} films={films} onSelect={onSelect}/>
};

export default PopularFilmsModule;