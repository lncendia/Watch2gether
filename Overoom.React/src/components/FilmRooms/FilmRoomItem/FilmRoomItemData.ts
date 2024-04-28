export interface FilmRoomItemData {
    id: string
    title: string
    year: number
    posterUrl: string
    ratingKp?: number
    ratingImdb?: number
    userRating: number
    description: string
    isSerial: boolean
    isPrivate: boolean
    viewersCount: number
}