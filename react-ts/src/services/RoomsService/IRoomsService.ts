import {RoomServer} from "./Models/RoomServer.ts";

export interface IRoomsService {
    createFilmRoom(body: CreateFilmRoomBody): Promise<RoomServer>

    createYoutubeRoom(body: CreateYoutubeRoomBody): Promise<RoomServer>

    connectFilmRoom(body: ConnectRoomBody): Promise<RoomServer>

    connectYoutubeRoom(body: ConnectRoomBody): Promise<RoomServer>
}