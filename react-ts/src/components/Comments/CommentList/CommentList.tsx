import CommentItem from "../CommentItem/CommentItem.tsx";
import {CommentData} from "../CommentItem/CommentData.ts";
import InfiniteScroll from "react-infinite-scroll-component";
import Spinner from "../../Common/Spinner/Spinner.tsx";

interface CommentListParams {
    comments: CommentData[];
    removeComment: (comment: CommentData) => void;
    hasMore: boolean,
    next: () => void
}

const CommentList = (props: CommentListParams) => {

    const scrollProps = {
        style: {overflow: "visible"},
        dataLength: props.comments.length,
        next: props.next,
        hasMore: props.hasMore,
        loader: <Spinner/>,
        className: "mt-4"
    }

    return (
        <InfiniteScroll {...scrollProps}>
            {props.comments.map(comment =>
                <CommentItem className="mb-4" key={comment.id} comment={comment}
                             removeComment={() => props.removeComment(comment)}/>
            )}
        </InfiniteScroll>
    );
};

export default CommentList;