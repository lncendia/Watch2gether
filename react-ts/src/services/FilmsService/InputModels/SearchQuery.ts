export interface SearchQuery {
    query?: string;
    genre?: string;
    person?: string;
    country?: string;
    serial?: boolean;
    playlistId?: string;
    minYear?: number,
    maxYear?: number,
    page?: number;
    countPerPage?: number;
}