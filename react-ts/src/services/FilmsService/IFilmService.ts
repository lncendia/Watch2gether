import {Films} from "./Models/Films.ts";
import {SearchQuery} from "./InputModels/SearchQuery.ts";

export interface IFilmService {
    search(query: SearchQuery): Promise<Films>
}