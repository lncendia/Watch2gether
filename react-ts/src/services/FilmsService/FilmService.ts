import * as yup from 'yup';
import {AxiosInstance} from "axios";
import {IFilmService} from "./IFilmService.ts";
import {Films} from "./Models/Films.ts";
import {SearchQuery} from "./InputModels/SearchQuery.ts";
import {FilmShort} from "./Models/FilmShort.ts";

// Валидация ответа сервера
const filmShortValidationSchema = yup.object({
    id: yup.string().uuid().required(),
    title: yup.string().required(),
    posterUrl: yup.string().required(),
    ratingKp: yup.number().nullable(),
    ratingImdb: yup.number().nullable(),
    userRating: yup.number().required(),
    description: yup.string().required(),
    isSerial: yup.boolean().required(),
    countSeasons: yup.number().integer().nullable(),
    countEpisodes: yup.number().integer().nullable(),
    genres: yup.array().of(yup.string()).required(),
});

// Валидация ответа сервера
const filmsValidationSchema = yup.object({
    films: yup.array().of(filmShortValidationSchema).required(),
    countPages: yup.number().required(),
});


// Экспорт класса FilmsService для его использования из других модулей
export class FilmService implements IFilmService {

    // Сервис для отправки запросов по API
    private axiosInstance: AxiosInstance

    constructor(axiosInstance: AxiosInstance) {
        this.axiosInstance = axiosInstance;
    }

    // Возвращает Promise, который содержит массив объектов EmployeeModel
    public async search(query: SearchQuery): Promise<Films> {

        // Отправка запроса к серверу для получения списка фильмов
        const response = await this.axiosInstance.get<Films>('Search', {params: query});

        // Валидация ответа сервера
        await filmsValidationSchema.validate(response.data);

        // Возвращаем данные
        return response.data
    }

    public async popular(count?: number): Promise<FilmShort[]> {

        // Отправка запроса к серверу для получения списка фильмов
        const response = await this.axiosInstance.get<FilmShort[]>('Popular', {params: {count: count}});

        // Валидация элементов массива
        response.data.forEach(f => filmShortValidationSchema.validateSync(f))

        // Возвращаем данные
        return response.data;
    }
}

