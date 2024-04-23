import {AxiosInstance} from "axios";
import {IPlaylistsService} from "./IPlaylistsService.ts";
import {Playlist, Playlists} from "./Models/Playlists.ts";
import {SearchQuery} from "./InputModels/SearchQuery.ts";
import {playlistSchema, playlistsSchema} from "./Validators/PlaylistsValidator.ts";

export class PlaylistsService implements IPlaylistsService {

    // Сервис для отправки запросов по API
    private axiosInstance: AxiosInstance

    constructor(axiosInstance: AxiosInstance) {
        this.axiosInstance = axiosInstance;
    }

    async get(id: string): Promise<Playlist> {

        // Отправка запроса к серверу для получения списка фильмов
        const response = await this.axiosInstance.get<Playlist>(`playlists/${id}`);

        // Валидация ответа сервера
        await playlistSchema.validate(response.data);

        // Возвращаем данные
        return response.data
    }

    // Возвращает Promise, который содержит массив объектов EmployeeModel
    public async search(query: SearchQuery): Promise<Playlists> {

        // Отправка запроса к серверу для получения списка фильмов
        const response = await this.axiosInstance.get<Playlists>('playlists', {params: query});

        // Валидация ответа сервера
        await playlistsSchema.validate(response.data);

        // Возвращаем данные
        return response.data
    }
}

