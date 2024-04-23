import {GetRatingsQuery} from "./InputModels/GetRatingsQuery.ts";
import {Ratings} from "./Models/Ratings.ts";
import {AddRatingBody} from "./InputModels/AddRatingBody.ts";
import {Profile} from "./Models/Profile.ts";

export interface IProfileService {

    profile(): Promise<Profile>

    ratings(query: GetRatingsQuery): Promise<Ratings>

    addRating(body: AddRatingBody): Promise<void>

    addToHistory(filmId: string): Promise<void>

    toggleWatchlist(filmId: string): Promise<void>

    changeAllows(body: ChangeAllowsBody): Promise<void>
}