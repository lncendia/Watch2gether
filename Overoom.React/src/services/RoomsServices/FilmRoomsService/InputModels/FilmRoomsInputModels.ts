import {RoomSearchInputModel} from "../../Common/InputModels/RoomsInputModels.ts";

export interface FilmRoomSearchInputModel extends RoomSearchInputModel {
    filmId?: string;
}

export interface CreateFilmRoomInputModel {
    open: boolean;
    filmId: string;
    cdnName: string;
}