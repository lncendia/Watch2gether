interface Room {
    id: string;
    viewersCount: number;
    isPrivate: boolean;
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
    isCodeNeeded: boolean;
}

export interface YoutubeRoomShort extends Room {
    videoAccess: boolean;
}

export interface YoutubeRoom extends YoutubeRoomShort {
    videoAccess: boolean;
    isCodeNeeded: boolean;
}

export interface Rooms<T extends Room> {
    rooms: T[];
    countPages: number;
}
