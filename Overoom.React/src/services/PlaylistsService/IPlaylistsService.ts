import {Playlist} from "./ViewModels/PlaylistViewModels.ts";
import {SearchInputModel} from "./InputModels/PlaylistInputModels.ts";
import {List} from "../Common/Models/List.ts";

export interface IPlaylistsService {
    search(query: SearchInputModel): Promise<List<Playlist>>
    get(id: string): Promise<Playlist>
}