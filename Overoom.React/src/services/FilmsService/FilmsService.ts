import {AxiosInstance} from "axios";
import {IFilmsService} from "./IFilmsService.ts";
import {Film, FilmShort} from "./ViewModels/FilmViewModels.ts";
import {SearchFilmInputModel} from "./InputModels/FilmInputModels.ts";
import {List} from "../Common/Models/List.ts";


// Экспорт класса FilmsService для его использования из других модулей
export class FilmsService implements IFilmsService {

    // Сервис для отправки запросов по API
    private axiosInstance: AxiosInstance

    constructor(axiosInstance: AxiosInstance) {
        this.axiosInstance = axiosInstance;
    }

    // Возвращает Promise, который содержит массив объектов EmployeeModel
    public async search(query: SearchFilmInputModel): Promise<List<FilmShort>> {

        // Отправка запроса к серверу для получения списка фильмов
        const response = await this.axiosInstance.get<List<FilmShort>>('films/search', {params: query});

        // Возвращаем данные
        return response.data
    }

    public async popular(count?: number): Promise<FilmShort[]> {

        // Отправка запроса к серверу для получения списка фильмов
        const response = await this.axiosInstance.get<FilmShort[]>('films/popular', {params: {count: count}});

        // Возвращаем данные
        return response.data;
    }

    public async get(id: string): Promise<Film> {

        // Отправка запроса к серверу для получения списка фильмов
        const response = await this.axiosInstance.get<Film>(`films/get/${id}`);

        // Возвращаем данные
        return response.data
    }
}

