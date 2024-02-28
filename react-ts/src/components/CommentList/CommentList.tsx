import React from 'react';
import CommentItem, {Comment} from "../CommentItem/CommentItem";

interface CommentListParams {
    comments: Comment[];
    removeComment: (comment: Comment) => void;
    className?: ''
}

const CommentList = (props: CommentListParams) => {
    return (
        <div className={props.className}>
            {props.comments.map(comment =>
                <CommentItem className="mb-4" key={comment.id} comment={comment}
                             removeComment={() => props.removeComment(comment)}/>
            )}
        </div>
    );
};

export default CommentList;