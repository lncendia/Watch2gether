import {RoomServer} from "./Models/RoomServer.ts";
import {CreateYoutubeRoomBody} from "./InputModels/CreateYoutubeRoomBody.ts";
import {ConnectRoomBody} from "./InputModels/ConnectRoomBody.ts";
import {RoomSearchQuery} from "./InputModels/SearchRoomQuery.ts";
import {YoutubeRoom, Rooms, YoutubeRoomShort} from "./Models/Rooms.ts";

export interface IYoutubeRoomsService {

    create(body: CreateYoutubeRoomBody): Promise<RoomServer>

    connect(body: ConnectRoomBody): Promise<RoomServer>

    search(query: RoomSearchQuery): Promise<Rooms<YoutubeRoomShort>>

    my(): Promise<YoutubeRoomShort[]>

    room(id: string): Promise<YoutubeRoom>
}