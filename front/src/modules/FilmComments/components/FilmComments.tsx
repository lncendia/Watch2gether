import React, {useState} from 'react';
import {Comment} from "../../../components/CommentItem/CommentItem";
import AddCommentForm from "./AddCommentForm";
import CommentList from "../../../components/CommentList/CommentList";

const FilmComments = () => {
    const [comments, setComments] = useState<Comment[]>([
        {id: "a", text: "хуита", userName: "Дыбил", createdAt: new Date(), avatarUrl: "/img/avatar.jpg", isOwner: true},
    ]);

    function addComment(text: string) {
        let comment: Comment = {
            id: Date.now().toString(),
            userName: "я",
            text: text,
            avatarUrl: "/img/avatar.jpg",
            isOwner: true,
            createdAt: new Date()
        };
        setComments([comment, ...comments])
    }

    function removeComment(comment: Comment) {
        setComments(comments.filter(p => p.id !== comment.id))
    }


    return (
        <div>
            <AddCommentForm callback={addComment}/>
            <CommentList comments={comments} removeComment={removeComment}/>
        </div>
    );
};

export default FilmComments;