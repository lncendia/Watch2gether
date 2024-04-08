export interface ConnectRoomInputModel {
    id: string;
    code?: string;
}

export interface RoomSearchInputModel {
    onlyPublic?: boolean;
    page?: number;
    countPerPage?: number;
}