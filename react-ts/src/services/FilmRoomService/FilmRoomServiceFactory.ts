import {IFilmRoomService} from "./IFilmRoomService.ts";
import {IFilmRoomServiceFactory} from "./IFilmRoomServiceFactory.ts";
import {FilmRoomService} from "./FilmRoomService.ts";

export class FilmRoomServiceFactory implements IFilmRoomServiceFactory {

    private readonly tokenFactory: () => Promise<string>
    private readonly authUrl: string


    constructor(tokenFactory: () => Promise<string>, authUrl: string) {
        this.tokenFactory = tokenFactory;
        this.authUrl = authUrl;
    }

    create(): IFilmRoomService {
        return new FilmRoomService(this.tokenFactory, this.authUrl)
    }
}