export interface Film {
    id: string;
    description: string;
    year: number;
    isSerial: boolean;
    title: string;
    posterUrl: string;
    ratingKp?: number;
    ratingImdb: number;
    userRating: number;
    userRatingsCount: number;
    userScore?: number;
    inWatchlist?: boolean;
    cdnList: Cdn[];
    countSeasons?: number;
    countEpisodes?: number;
    genres: string[];
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