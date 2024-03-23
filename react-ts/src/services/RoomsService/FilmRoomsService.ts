import {IFilmRoomsService} from "./IFilmRoomsService.ts";
import {RoomServer} from "./Models/RoomServer.ts";
import {AxiosInstance} from "axios";
import {roomServerSchema} from "./Validators/RoomServerValidator.ts";
import {CreateFilmRoomBody} from "./InputModels/CreateFilmRoomBody.ts";
import {ConnectRoomBody} from "./InputModels/ConnectRoomBody.ts";
import {FilmRoomSearchQuery} from "./InputModels/SearchRoomQuery.ts";
import {Rooms, FilmRoom} from "./Models/Rooms.ts";
import {filmRoomSchema, filmRoomsSchema} from "./Validators/RoomsValidator.ts";

export class FilmRoomsService implements IFilmRoomsService {

    // Сервис для отправки запросов по API
    private axiosInstance: AxiosInstance

    constructor(axiosInstance: AxiosInstance) {
        this.axiosInstance = axiosInstance;
    }

    async room(id: string): Promise<FilmRoom> {

        const response = await this.axiosInstance.get<FilmRoom>(`filmRooms/room/${id}`);

        // Валидация ответа сервера
        await filmRoomSchema.validate(response.data)

        return response.data
    }

    async search(query: FilmRoomSearchQuery): Promise<Rooms<FilmRoom>> {

        const response = await this.axiosInstance.get<Rooms<FilmRoom>>('filmRooms/search', {params: query});

        // Валидация ответа сервера
        await filmRoomsSchema.validate(response.data)

        return response.data
    }

    async create(body: CreateFilmRoomBody): Promise<RoomServer> {

        const response = await this.axiosInstance.put<RoomServer>('filmRooms/create', body);

        // Валидация ответа сервера
        await roomServerSchema.validate(response.data);

        return response.data
    }


    async connect(body: ConnectRoomBody): Promise<RoomServer> {

        const response = await this.axiosInstance.post<RoomServer>('filmRooms/connect', body);

        // Валидация ответа сервера
        await roomServerSchema.validate(response.data);

        return response.data
    }
}