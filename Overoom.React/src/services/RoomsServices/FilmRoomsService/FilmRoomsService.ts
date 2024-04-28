import {IFilmRoomsService} from "./IFilmRoomsService.ts";
import {AxiosInstance} from "axios";
import {FilmRoom, FilmRoomShort} from "./Models/FilmRoomsViewModels.ts";
import {CreateFilmRoomInputModel, FilmRoomSearchInputModel} from "./InputModels/FilmRoomsInputModels.ts";
import {List} from "../../Common/Models/List.ts";
import {RoomServer} from "../Common/ViewModels/RoomsViewModels.ts";
import {ConnectRoomInputModel} from "../Common/InputModels/RoomsInputModels.ts";

export class FilmRoomsService implements IFilmRoomsService {

    // Сервис для отправки запросов по API
    private axiosInstance: AxiosInstance

    constructor(axiosInstance: AxiosInstance) {
        this.axiosInstance = axiosInstance;
    }

    async room(id: string): Promise<FilmRoom> {

        const response = await this.axiosInstance.get<FilmRoom>(`filmRooms/room/${id}`);

        return response.data
    }

    async search(query: FilmRoomSearchInputModel): Promise<List<FilmRoomShort>> {

        const response = await this.axiosInstance.get<List<FilmRoomShort>>('filmRooms/search', {params: query});

        return response.data
    }

    async my(): Promise<FilmRoomShort[]> {

        const response = await this.axiosInstance.get<FilmRoomShort[]>('filmRooms/my');

        return response.data
    }

    async create(body: CreateFilmRoomInputModel): Promise<RoomServer> {

        const response = await this.axiosInstance.put<RoomServer>('filmRooms/create', body);

        return response.data
    }


    async connect(body: ConnectRoomInputModel): Promise<RoomServer> {

        const response = await this.axiosInstance.post<RoomServer>('filmRooms/connect', body);

        return response.data
    }
}