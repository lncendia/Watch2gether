import {IFilmRoomService} from "./IFilmRoomService.ts";

export interface IFilmRoomServiceFactory {
    create(): IFilmRoomService
}