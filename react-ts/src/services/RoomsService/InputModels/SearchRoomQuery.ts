export interface RoomSearchQuery {
    onlyPublic?: boolean;
    page?: number;
    countPerPage?: number;
}

export interface FilmRoomSearchQuery extends RoomSearchQuery {
    filmId?: string;
}
