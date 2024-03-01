import {GetRatingsQuery} from "./InputModels/GetRatingsQuery.ts";
import {Ratings} from "./Models/Ratings.ts";
import {AddRatingData} from "./InputModels/AddRatingData.ts";
import {ChangeAllowsData} from "./InputModels/ChangeAllowsData.ts";

export interface IUserService {

    Ratings(query: GetRatingsQuery): Promise<Ratings>

    AddRating(body: AddRatingData): Promise<void>

    ToggleWatchlist(filmId: string): Promise<void>

    ChangeAllows(body: ChangeAllowsData): Promise<void>
}