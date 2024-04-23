import {UserFilm} from "./Profile.ts";

export interface Ratings {
    ratings: Rating[];
    countPages: number;
}

export interface Rating extends UserFilm {
    score: number;
}
