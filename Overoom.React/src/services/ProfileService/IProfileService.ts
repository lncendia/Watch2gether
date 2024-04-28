import {Profile, Rating} from "./ViewModels/ProfileViewModels.ts";
import {List} from "../Common/Models/List.ts";
import {AddRatingInputModel, ChangeAllowsInputModel, GetRatingsInputModel} from "./InputModels/ProfileInputModels.ts";


export interface IProfileService {

    profile(): Promise<Profile>

    ratings(query: GetRatingsInputModel): Promise<List<Rating>>

    addRating(body: AddRatingInputModel): Promise<void>

    addToHistory(filmId: string): Promise<void>

    toggleWatchlist(filmId: string): Promise<void>

    changeAllows(body: ChangeAllowsInputModel): Promise<void>
}