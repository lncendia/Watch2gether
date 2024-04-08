export interface FilmShort {
    id: string
    title: string
    year: number
    posterUrl: string
    ratingKp?: number
    ratingImdb?: number
    userRating: number
    description: string
    isSerial: boolean
    countSeasons?: number
    countEpisodes?: number
    genres: string[]
}

export interface Film extends FilmShort {
    userRatingsCount: number;
    userScore?: number;
    inWatchlist?: boolean;
    cdnList: Cdn[];
    countSeasons?: number;
    countEpisodes?: number;
    countries: string[];
    directors: string[];
    screenWriters: string[];
    actors: Actor[];
}

export interface Actor {
    name: string;
    description?: string;
}

export interface Cdn {
    url: string;
    cdn: string;
    quality: string;
}