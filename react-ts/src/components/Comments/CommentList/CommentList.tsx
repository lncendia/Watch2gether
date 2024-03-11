import CommentItem from "../CommentItem/CommentItem.tsx";
import {CommentData} from "../CommentItem/CommentData.ts";

interface CommentListParams {
    comments: CommentData[];
    removeComment: (comment: CommentData) => void;
}

const CommentList = (props: CommentListParams) => {
    return (
        <>
            {props.comments.map(comment =>
                <CommentItem className="mb-4" key={comment.id} comment={comment}
                             removeComment={() => props.removeComment(comment)}/>
            )}
        </>
    );
};

export default CommentList;