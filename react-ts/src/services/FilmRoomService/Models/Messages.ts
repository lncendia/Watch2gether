export interface Message {
    id: string;
    userId: string;
    roomId: string;
    createdAt: Date;
    text: string;
}

export interface Messages {
    messages: Message[];
    count: number;
}