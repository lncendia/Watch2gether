export interface YoutubeRoomData {
    id: string;
    title: string;
    cdnName: string;
    cdnUrl: string;
    isSerial: boolean;
    ownerId: string;
    currentId: string;
    filmId: string;
    posterUrl: string;
    year: number;
    userRating: number;
    userRatingsCount: number;
    userScore?: number;
    ratingKp?: number;
    ratingImdb?: number;
    description: string;
    code?: string
}