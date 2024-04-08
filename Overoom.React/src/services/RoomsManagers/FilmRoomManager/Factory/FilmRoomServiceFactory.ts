import {IFilmRoomManager} from "../IFilmRoomManager.ts";
import {IFilmRoomManagerFactory} from "./IFilmRoomManagerFactory.ts";
import {FilmRoomManager} from "../FilmRoomManager.ts";

export class FilmRoomManagerFactory implements IFilmRoomManagerFactory {

    private readonly tokenFactory: () => Promise<string>
    private readonly authUrl: string


    constructor(tokenFactory: () => Promise<string>, authUrl: string) {
        this.tokenFactory = tokenFactory;
        this.authUrl = authUrl;
    }

    create(): IFilmRoomManager {
        return new FilmRoomManager(this.tokenFactory, this.authUrl)
    }
}