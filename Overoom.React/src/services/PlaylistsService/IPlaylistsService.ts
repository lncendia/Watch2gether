import {Playlist, Playlists} from "./Models/Playlists.ts";
import {SearchQuery} from "./InputModels/SearchQuery.ts";

export interface IPlaylistsService {
    search(query: SearchQuery): Promise<Playlists>
    get(id: string): Promise<Playlist>
}