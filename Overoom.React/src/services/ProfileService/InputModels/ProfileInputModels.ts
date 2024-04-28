export interface GetRatingsInputModel {
    countPerPage?: number;
    page?: number;
}

export interface ChangeAllowsInputModel {
    beep: boolean;
    scream: boolean;
    change: boolean;
}

export interface AddRatingInputModel {
    filmId: string;
    score: number;
}