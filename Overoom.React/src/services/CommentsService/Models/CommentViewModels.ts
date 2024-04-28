export interface Comment {
    id: string;
    username: string;
    text: string;
    avatarUrl?: string;
    createdAt: string;
    isUserComment: boolean;
}