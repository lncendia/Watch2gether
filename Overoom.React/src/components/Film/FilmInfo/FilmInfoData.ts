export interface FilmInfoData {
    id: string;
    description: string;
    year: number;
    title: string;
    posterUrl: string;
    ratingKp?: number;
    ratingImdb?: number;
    type: [string, string?];
    genres: [string, string?][];
    countries: [string, string?][];
    directors: [string, string?][];
    screenWriters: [string, string?][];
    actors: [string, string?][];
}