import {Film, Films, FilmShort} from "./Models/Films.ts";
import {SearchFilmQuery} from "./InputModels/SearchFilmQuery.ts";

export interface IFilmsService {
    search(query: SearchFilmQuery): Promise<Films>

    popular(count?: number): Promise<FilmShort[]>

    get(id: string): Promise<Film>
}