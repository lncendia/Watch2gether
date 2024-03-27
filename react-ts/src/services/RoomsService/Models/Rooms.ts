interface Room {
    id: string;
    viewersCount: number;
    isCodeNeeded: boolean;
}

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
}

export interface YoutubeRoom extends Room {
    videoAccess: boolean;
}

export interface Rooms<T extends Room> {
    rooms: T[];
    countPages: number;
}
