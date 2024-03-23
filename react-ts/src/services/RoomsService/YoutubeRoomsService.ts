import {IYoutubeRoomsService} from "./IYoutubeRoomsService.ts";
import {RoomServer} from "./Models/RoomServer.ts";
import {AxiosInstance} from "axios";
import {roomServerSchema} from "./Validators/RoomServerValidator.ts";
import {YoutubeRoom, Rooms} from "./Models/Rooms.ts";
import {youtubeRoomsSchema} from "./Validators/RoomsValidator.ts";
import {CreateYoutubeRoomBody} from "./InputModels/CreateYoutubeRoomBody.ts";
import {ConnectRoomBody} from "./InputModels/ConnectRoomBody.ts";
import {RoomSearchQuery} from "./InputModels/SearchRoomQuery.ts";

export class YoutubeRoomsService implements IYoutubeRoomsService {

    // Сервис для отправки запросов по API
    private axiosInstance: AxiosInstance

    constructor(axiosInstance: AxiosInstance) {
        this.axiosInstance = axiosInstance;
    }

    async search(query: RoomSearchQuery): Promise<Rooms<YoutubeRoom>> {

        const response = await this.axiosInstance.get<Rooms<YoutubeRoom>>('youtubeRooms/search', {params: query});

        // Валидация ответа сервера
        await youtubeRoomsSchema.validate(response.data)

        return response.data
    }

    async create(body: CreateYoutubeRoomBody): Promise<RoomServer> {

        const response = await this.axiosInstance.put<RoomServer>('youtubeRooms/createYoutubeRoom', body);

        // Валидация ответа сервера
        await roomServerSchema.validate(response.data);

        return response.data
    }

    async connect(body: ConnectRoomBody): Promise<RoomServer> {

        const response = await this.axiosInstance.put<RoomServer>('youtubeRooms/connectYoutubeRoom', body);

        // Валидация ответа сервера
        await roomServerSchema.validate(response.data);

        return response.data
    }
}