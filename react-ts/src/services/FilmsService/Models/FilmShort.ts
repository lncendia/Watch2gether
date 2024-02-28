export interface FilmShort {
    id: string
    title: string
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