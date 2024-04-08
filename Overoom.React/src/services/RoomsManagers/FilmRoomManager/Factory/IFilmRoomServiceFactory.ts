import {IFilmRoomManager} from "../IFilmRoomManager.ts";

export interface IFilmRoomManagerFactory {
    create(): IFilmRoomManager
}