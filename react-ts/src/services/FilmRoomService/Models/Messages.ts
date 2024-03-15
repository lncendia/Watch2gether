export interface Message {
    id: string;
    userId: string;
    createdAt: Date;
    text: string;
}

export interface Messages {
    messages: Message[];
    count: number;
}