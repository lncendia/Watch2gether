export interface Room {
    id: string;
    viewersCount: number;
    isPrivate: boolean;
}

export interface RoomServer {
    id: string
    url: string
    code?: string
}