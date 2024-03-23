export interface RoomSearchQuery {
    onlyPublic?: boolean;
    onlyMy?: boolean;
    page?: number;
    countPerPage?: number;
}

export interface FilmRoomSearchQuery extends RoomSearchQuery {
    filmId?: string;
}
