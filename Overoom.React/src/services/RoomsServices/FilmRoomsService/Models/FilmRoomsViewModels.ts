import {Room} from "../../Common/ViewModels/RoomsViewModels.ts";

export interface FilmRoomShort extends Room {
    filmId: string;
    title: string;
    posterUrl: string;
    year: number;
    userRating: number;
    ratingKp?: number;
    ratingImdb?: number;
    description: string;
    isSerial: boolean;
}

export interface FilmRoom extends FilmRoomShort {
    userRatingsCount: number;
    userScore?: number;
    isCodeNeeded: boolean;
}
