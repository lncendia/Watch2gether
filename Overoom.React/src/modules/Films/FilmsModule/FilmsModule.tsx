import {useCallback, useEffect, useState} from 'react';
import {useInjection} from "inversify-react";
import {IFilmsService} from "../../../services/FilmsService/IFilmsService.ts";
import {useNavigate} from "react-router-dom";
import FilmsCatalog from "../../../components/Films/FilmsCatalog/FilmsCatalog.tsx";
import {FilmItemData} from "../../../components/Films/FilmItem/FilmItemData.ts";
import NoData from "../../../UI/NoData/NoData.tsx";

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
            setHasMore(response.totalPages > 1)
            setFilms(response.list)
        };

        processFilms().then()
    }, [filmsService, props]);

    const next = useCallback(async () => {
        const response = await filmsService.search({
            ...props,
            minYear: props.year,
            maxYear: props.year,
            page: page
        })
        setPage(page + 1);
        setHasMore(response.totalPages !== page)
        setFilms(prev => [...prev, ...response.list])
    }, [props, page, filmsService])

    const onSelect = useCallback((film: FilmItemData) => {
        navigate('/film', {state: {id: film.id}})
    }, [navigate])

    if (films.length === 0) return <NoData className={props.className} text="Фильмы не найдены"/>

    return <FilmsCatalog className={props.className} hasMore={hasMore} next={next} genre={props.genre} films={films}
                         onSelect={onSelect}
                         typeSelected={props.serial !== undefined}/>
};

export default FilmsModule;