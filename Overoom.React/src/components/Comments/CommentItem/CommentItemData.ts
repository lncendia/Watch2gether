export interface CommentItemData {
    id: string;
    username: string;
    text: string;
    avatarUrl?: string;
    createdAt: Date;
    isUserComment: boolean;
}