export interface Films {
    films: FilmShort[];
    countPages: number;
}

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