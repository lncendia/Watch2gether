import {useEffect, useState} from 'react';
import FilmsCatalog from "../../components/FilmsCatalog/FilmsCatalog.tsx";
import InfiniteScroll from "react-infinite-scroll-component";
import Spinner from "../../components/Spinner/Spinner.tsx";
import {FilmShort} from "../../services/FilmsService/Models/FilmShort.ts";
import {useInjection} from "inversify-react";
import {IFilmService} from "../../services/FilmsService/IFilmService.ts";
import {AxiosError} from "axios";
import {ValidationError} from "yup";

const FilmsModule = ({genre, className}: { genre?: string, className?: string }) => {

    const [films, setFilms] = useState<FilmShort[]>([]);
    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(true);
    const [errorMessage, setErrorMessage] = useState<string | undefined>(undefined);
    const filmService = useInjection<IFilmService>('FilmService');


    useEffect(() => {
        const processFilms = async () => {
            try {
                const response = await filmService.search({countPerPage: 15, genre: genre, page: 1})
                setPage(2);
                setHasMore(response.countPages !== 1)
                setFilms(response.films)
            } catch (e) {
                if (e instanceof AxiosError) {
                    setErrorMessage("Не удалось отправить запрос на сервер");
                    setHasMore(false)
                } else if (e instanceof ValidationError) {
                    setErrorMessage("Данные получены в некорректном формате");
                    setHasMore(false)
                } else {
                    throw e;
                }
            }
        };

        processFilms().then()
    }, [genre]); // Эффект будет вызываться при каждом изменении `genre`

    const onBottom = () => {
        const processFilms = async () => {
            try {
                const response = await filmService.search({countPerPage: 15, genre: genre, page: page})
                setPage(page + 1);
                setHasMore(response.countPages !== page)
                setFilms([...films, ...response.films])
            } catch (e) {
                if (e instanceof AxiosError) {
                    setErrorMessage("Не удалось отправить запрос на сервер");
                    setHasMore(false)
                } else if (e instanceof ValidationError) {
                    setErrorMessage("Данные получены в некорректном формате");
                    setHasMore(false)
                } else {
                    throw e;
                }
            }
        };

        processFilms().then()
    }

    const scrollProps = {
        style: {overflow: "hidden"},
        dataLength: films.length,
        next: onBottom,
        hasMore: hasMore,
        loader: <Spinner/>,
        endMessage: <p>No more data to load.</p>,
        className: className
    }

    return (
        <>
            <InfiniteScroll {...scrollProps}>
                {errorMessage && <div style={{color: 'red'}}>{errorMessage}</div>}
                <FilmsCatalog genre={genre} films={films}/>
            </InfiniteScroll>
        </>
    );
};

export default FilmsModule;