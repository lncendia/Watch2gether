import {IFilmRoomsService} from "./IFilmRoomsService.ts";
import {RoomServer} from "./Models/RoomServer.ts";
import {AxiosInstance} from "axios";
import {CreateFilmRoomBody} from "./InputModels/CreateFilmRoomBody.ts";
import {ConnectRoomBody} from "./InputModels/ConnectRoomBody.ts";
import {FilmRoomSearchQuery} from "./InputModels/SearchRoomQuery.ts";
import {Rooms, FilmRoom, FilmRoomShort} from "./Models/Rooms.ts";
import {filmRoomSchema, filmRoomShortSchema, filmRoomsSchema, roomServerSchema} from "./Validators/RoomsValidator.ts";

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

    async search(query: FilmRoomSearchQuery): Promise<Rooms<FilmRoomShort>> {

        const response = await this.axiosInstance.get<Rooms<FilmRoomShort>>('filmRooms/search', {params: query});

        // Валидация ответа сервера
        await filmRoomsSchema.validate(response.data)

        return response.data
    }

    async my(): Promise<FilmRoomShort[]> {

        const response = await this.axiosInstance.get<FilmRoomShort[]>('filmRooms/my');

        response.data.forEach(v=>filmRoomShortSchema.validateSync(v))

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