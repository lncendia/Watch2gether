export interface Comments {
    comments: Comment[];
    countPages: number;
}

export interface Comment {
    id: string;
    username: string;
    text: string;
    avatarUrl?: string;
    createdAt: Date;
    isUserComment: boolean;
}