import {useEffect, useState} from 'react';
import {FilmShort} from "../../services/FilmsService/Models/FilmShort.ts";
import {useInjection} from "inversify-react";
import {IFilmService} from "../../services/FilmsService/IFilmService.ts";
import {AxiosError} from "axios";
import {ValidationError} from "yup";
import PopularFilmsSlider from "../../components/PopularFilmsSlider/PopularFilmsSlider.tsx";

const PopularFilmsModule = ({className}: { className?: string }) => {

    const [films, setFilms] = useState<FilmShort[]>([]);
    const [errorMessage, setErrorMessage] = useState<string | undefined>(undefined);
    const filmService = useInjection<IFilmService>('FilmService');


    useEffect(() => {
        const processFilms = async () => {
            try {
                const response = await filmService.popular()
                setFilms(response)
            } catch (e) {
                if (e instanceof AxiosError) {
                    setErrorMessage("Не удалось отправить запрос на сервер");
                } else if (e instanceof ValidationError) {
                    setErrorMessage("Данные получены в некорректном формате");
                } else {
                    throw e;
                }
            }
        };

        processFilms().then()
    }, []); // Эффект будет вызываться при каждом изменении `genre`


    return (
        <div className={className}>
            <h3 className={"mb-3"}>Популярное сейчас</h3>
            {errorMessage && <div style={{color: 'red'}}>{errorMessage}</div>}
            <PopularFilmsSlider films={films}/>
        </div>
    );
};

export default PopularFilmsModule;