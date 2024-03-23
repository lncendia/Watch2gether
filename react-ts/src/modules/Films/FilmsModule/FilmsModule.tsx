import {useEffect, useState} from 'react';
import InfiniteScroll from "react-infinite-scroll-component";
import Spinner from "../../../components/Common/Spinner/Spinner.tsx";
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

    const onBottom = () => {
        const processFilms = async () => {
            const response = await filmsService.search({
                ...props,
                minYear: props.year,
                maxYear: props.year,
                page: page
            })
            setPage(page + 1);
            setHasMore(response.countPages !== page)
            setFilms([...films, ...response.films])
        };

        processFilms().then()
    }

    const onFilmSelect = (film: FilmItemData) => {
        navigate('/film', {state: {id: film.id}})
    }

    const scrollProps = {
        dataLength: films.length,
        next: onBottom,
        hasMore: hasMore,
        loader: <Spinner/>,
        className: props.className
    }

    return (
        <InfiniteScroll {...scrollProps}>
            <FilmsCatalog genre={props.genre} films={films} onFilmSelect={onFilmSelect}
                          typeSelected={props.serial !== undefined}/>
        </InfiniteScroll>
    );
};

export default FilmsModule;