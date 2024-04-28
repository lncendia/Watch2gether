export interface AddInputModel {
    filmId: string;
    text: string;
}
export interface GetInputModel {
    filmId: string;
    countPerPage?: number;
    page?: number;
}