interface Room {
    id: string;
    viewersCount: number;
    isCodeNeeded: boolean;
}

export interface FilmRoom extends Room {
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

export interface YoutubeRoom extends Room {
    videoAccess: boolean;
}

export interface Rooms<T extends Room> {
    rooms: T[];
    countPages: number;
}
