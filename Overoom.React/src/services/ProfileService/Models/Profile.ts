export interface Profile {
    allows: Allows;
    watchlist: UserFilm[];
    history: UserFilm[];
    genres: string[];
}

export interface UserFilm {
    id: string;
    title: string;
    year: number;
    posterUrl: string;
    ratingKp?: number
    ratingImdb?: number
}

export interface Allows {
    beep: boolean;
    scream: boolean;
    change: boolean;
}