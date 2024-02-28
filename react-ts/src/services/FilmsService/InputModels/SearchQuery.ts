export interface SearchQuery {
    query?: string;
    genre?: string;
    person?: string;
    country?: string;
    serial?: boolean;
    playlistId?: string;
    page: number;
    countPerPage: number;
}