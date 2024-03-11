import {Films, FilmShort} from "./Models/Films.ts";
import {SearchQuery} from "./InputModels/SearchQuery.ts";
import {Film} from "./Models/Film.ts";

export interface IFilmsService {
    search(query: SearchQuery): Promise<Films>

    popular(count?: number): Promise<FilmShort[]>

    get(id: string): Promise<Film>
}