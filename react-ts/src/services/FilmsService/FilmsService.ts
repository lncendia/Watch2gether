import {AxiosInstance} from "axios";
import {IFilmsService} from "./IFilmsService.ts";
import {Films, FilmShort} from "./Models/Films.ts";
import {SearchFilmQuery} from "./InputModels/SearchFilmQuery.ts";
import {filmsSchema, filmShortSchema} from "./Validators/FilmsValidator.ts"
import {Film} from "./Models/Film.ts";
import {filmSchema} from "./Validators/FilmValidator.ts";


// Экспорт класса FilmsService для его использования из других модулей
export class FilmsService implements IFilmsService {

    // Сервис для отправки запросов по API
    private axiosInstance: AxiosInstance

    constructor(axiosInstance: AxiosInstance) {
        this.axiosInstance = axiosInstance;
    }

    // Возвращает Promise, который содержит массив объектов EmployeeModel
    public async search(query: SearchFilmQuery): Promise<Films> {

        // Отправка запроса к серверу для получения списка фильмов
        const response = await this.axiosInstance.get<Films>('films/search', {params: query});

        // Валидация ответа сервера
        await filmsSchema.validate(response.data);

        // Возвращаем данные
        return response.data
    }

    public async popular(count?: number): Promise<FilmShort[]> {

        // Отправка запроса к серверу для получения списка фильмов
        const response = await this.axiosInstance.get<FilmShort[]>('films/popular', {params: {count: count}});

        // Валидация элементов массива
        response.data.forEach(f => filmShortSchema.validateSync(f))

        // Возвращаем данные
        return response.data;
    }

    public async get(id: string): Promise<Film> {

        // Отправка запроса к серверу для получения списка фильмов
        const response = await this.axiosInstance.get<Film>(`films/get/${id}`);

        // Валидация ответа сервера
        await filmSchema.validate(response.data);

        // Возвращаем данные
        return response.data
    }
}

