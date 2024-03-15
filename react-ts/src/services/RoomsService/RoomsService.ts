import {IRoomsService} from "./IRoomsService.ts";
import {RoomServer} from "./Models/RoomServer.ts";
import {AxiosInstance} from "axios";
import {roomServerSchema} from "./Validators/RoomServerValidator.ts";

export class RoomsService implements IRoomsService {

    // Сервис для отправки запросов по API
    private axiosInstance: AxiosInstance

    constructor(axiosInstance: AxiosInstance) {
        this.axiosInstance = axiosInstance;
    }

    async createFilmRoom(body: CreateFilmRoomBody): Promise<RoomServer> {

        const response = await this.axiosInstance.put('rooms/createFilmRoom', body);

        // Валидация ответа сервера
        await roomServerSchema.validate(response.data);

        return response.data
    }

    async createYoutubeRoom(body: CreateYoutubeRoomBody): Promise<RoomServer> {

        const response = await this.axiosInstance.put('rooms/createYoutubeRoom', body);

        // Валидация ответа сервера
        await roomServerSchema.validate(response.data);

        return response.data
    }

    async connectFilmRoom(body: ConnectRoomBody): Promise<RoomServer> {

        const response = await this.axiosInstance.put('rooms/connectFilmRoom', body);

        // Валидация ответа сервера
        await roomServerSchema.validate(response.data);

        return response.data
    }

    async connectYoutubeRoom(body: ConnectRoomBody): Promise<RoomServer> {

        const response = await this.axiosInstance.put('rooms/connectYoutubeRoom', body);

        // Валидация ответа сервера
        await roomServerSchema.validate(response.data);

        return response.data
    }
}