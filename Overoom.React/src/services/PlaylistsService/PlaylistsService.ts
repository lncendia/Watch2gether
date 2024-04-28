import {AxiosInstance} from "axios";
import {IPlaylistsService} from "./IPlaylistsService.ts";
import {List} from "../Common/Models/List.ts";
import {Playlist} from "./ViewModels/PlaylistViewModels.ts";
import {SearchInputModel} from "./InputModels/PlaylistInputModels.ts";

export class PlaylistsService implements IPlaylistsService {

    // Сервис для отправки запросов по API
    private axiosInstance: AxiosInstance

    constructor(axiosInstance: AxiosInstance) {
        this.axiosInstance = axiosInstance;
    }

    async get(id: string): Promise<Playlist> {

        // Отправка запроса к серверу для получения списка фильмов
        const response = await this.axiosInstance.get<Playlist>(`playlists/${id}`);

        // Возвращаем данные
        return response.data
    }

    // Возвращает Promise, который содержит массив объектов EmployeeModel
    public async search(query: SearchInputModel): Promise<List<Playlist>> {

        // Отправка запроса к серверу для получения списка фильмов
        const response = await this.axiosInstance.get<List<Playlist>>('playlists', {params: query});

        // Возвращаем данные
        return response.data
    }
}

