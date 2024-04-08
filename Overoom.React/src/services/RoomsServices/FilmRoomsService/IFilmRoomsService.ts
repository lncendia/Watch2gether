import {CreateFilmRoomInputModel, FilmRoomSearchInputModel} from "./InputModels/FilmRoomsInputModels.ts";
import {RoomServer} from "../Common/ViewModels/RoomsViewModels.ts";
import {ConnectRoomInputModel} from "../Common/InputModels/RoomsInputModels.ts";
import {FilmRoom, FilmRoomShort} from "./Models/FilmRoomsViewModels.ts";
import {List} from "../../Common/Models/List.ts";

export interface IFilmRoomsService {

    create(body: CreateFilmRoomInputModel): Promise<RoomServer>

    connect(body: ConnectRoomInputModel): Promise<RoomServer>

    search(query: FilmRoomSearchInputModel): Promise<List<FilmRoomShort>>

    my(): Promise<FilmRoomShort[]>

    room(id: string): Promise<FilmRoom>
}