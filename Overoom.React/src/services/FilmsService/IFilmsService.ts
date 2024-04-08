import {Film, FilmShort} from "./ViewModels/FilmViewModels.ts";
import {SearchFilmInputModel} from "./InputModels/FilmInputModels.ts";
import {List} from "../Common/Models/List.ts";

export interface IFilmsService {
    search(query: SearchFilmInputModel): Promise<List<FilmShort>>

    popular(count?: number): Promise<FilmShort[]>

    get(id: string): Promise<Film>
}