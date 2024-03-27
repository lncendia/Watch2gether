import {RoomServer} from "./Models/RoomServer.ts";
import {CreateFilmRoomBody} from "./InputModels/CreateFilmRoomBody.ts";
import {ConnectRoomBody} from "./InputModels/ConnectRoomBody.ts";
import {FilmRoomSearchQuery} from "./InputModels/SearchRoomQuery.ts";
import {FilmRoom, FilmRoomShort, Rooms} from "./Models/Rooms.ts";

export interface IFilmRoomsService {

    create(body: CreateFilmRoomBody): Promise<RoomServer>

    connect(body: ConnectRoomBody): Promise<RoomServer>

    search(query: FilmRoomSearchQuery): Promise<Rooms<FilmRoomShort>>

    room(id: string): Promise<FilmRoom>
}