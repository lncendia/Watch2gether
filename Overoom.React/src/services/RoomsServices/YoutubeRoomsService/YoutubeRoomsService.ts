import {IYoutubeRoomsService} from "./IYoutubeRoomsService.ts";
import {AxiosInstance} from "axios";
import {ConnectRoomInputModel, RoomSearchInputModel} from "../Common/InputModels/RoomsInputModels.ts";
import {List} from "../../Common/Models/List.ts";
import {YoutubeRoom, YoutubeRoomShort} from "./ViewModels/YoutubeRoomsViewModels.ts";
import {CreateYoutubeRoomInputModel} from "./InputModels/YoutubeRoomsInputModels.ts";
import {RoomServer} from "../Common/ViewModels/RoomsViewModels.ts";

export class YoutubeRoomsService implements IYoutubeRoomsService {

    // Сервис для отправки запросов по API
    private axiosInstance: AxiosInstance

    constructor(axiosInstance: AxiosInstance) {
        this.axiosInstance = axiosInstance;
    }

    async search(query: RoomSearchInputModel): Promise<List<YoutubeRoomShort>> {

        const response = await this.axiosInstance.get<List<YoutubeRoomShort>>('youtubeRooms/search', {params: query});

        return response.data
    }

    async create(body: CreateYoutubeRoomInputModel): Promise<RoomServer> {

        const response = await this.axiosInstance.put<RoomServer>('youtubeRooms/createYoutubeRoom', body);

        return response.data
    }

    async connect(body: ConnectRoomInputModel): Promise<RoomServer> {

        const response = await this.axiosInstance.put<RoomServer>('youtubeRooms/connectYoutubeRoom', body);

        return response.data
    }

    async my(): Promise<YoutubeRoomShort[]> {

        const response = await this.axiosInstance.get<YoutubeRoomShort[]>('youtubeRooms/my');

        return response.data
    }

    async room(id: string): Promise<YoutubeRoom> {
        const response = await this.axiosInstance.get<YoutubeRoom>(`youtubeRooms/room/${id}`);

        return response.data
    }
}