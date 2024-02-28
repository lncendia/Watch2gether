import {Films} from "./Models/Films.ts";
import {SearchQuery} from "./InputModels/SearchQuery.ts";
import {FilmShort} from "./Models/FilmShort.ts";

export interface IFilmService {
    search(query: SearchQuery): Promise<Films>
    popular(count?: number): Promise<FilmShort[]>
}