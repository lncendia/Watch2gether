import {useCallback, useEffect, useState} from 'react';
import {useInjection} from "inversify-react";
import {IFilmsService} from "../../../services/FilmsService/IFilmsService.ts";
import {useNavigate} from "react-router-dom";
import FilmsCatalog from "../../../components/Films/FilmsCatalog/FilmsCatalog.tsx";
import {FilmItemData} from "../../../components/Films/FilmItem/FilmItemData.ts";

interface FilmsModuleProps {
    genre?: string
    person?: string
    country?: string
    serial?: boolean
    playlistId?: string
    year?: number
    className?: string
}

const FilmsModule = (props: FilmsModuleProps) => {

    const [films, setFilms] = useState<FilmItemData[]>([]);
    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(false);
    const filmsService = useInjection<IFilmsService>('FilmsService');

    // Навигационный хук
    const navigate = useNavigate();


    useEffect(() => {
        const processFilms = async () => {
            const response = await filmsService.search({
                ...props,
                minYear: props.year,
                maxYear: props.year,
            })

            setPage(2);
            setHasMore(response.countPages > 1)
            setFilms(response.films)
        };

        processFilms().then()
    }, [props]); // Эффект будет вызываться при каждом изменении `genre`

    const next = useCallback(async () => {
        const response = await filmsService.search({
            ...props,
            minYear: props.year,
            maxYear: props.year,
            page: page
        })
        setPage(page + 1);
        setHasMore(response.countPages !== page)
        setFilms(prev => [...prev, ...response.films])
    }, [props, page, filmsService])

    const onSelect = useCallback((film: FilmItemData) => {
        navigate('/film', {state: {id: film.id}})
    }, [navigate])

    return (
        <FilmsCatalog hasMore={hasMore} next={next} genre={props.genre} films={films} onSelect={onSelect}
                      typeSelected={props.serial !== undefined}/>
    );
};

export default FilmsModule;