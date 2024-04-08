import CommentItem from "../CommentItem/CommentItem.tsx";
import {CommentItemData} from "../CommentItem/CommentItemData.ts";
import InfiniteScroll from "react-infinite-scroll-component";
import Spinner from "../../../UI/Spinner/Spinner.tsx";

interface CommentListParams {
    comments: CommentItemData[];
    removeComment: (comment: CommentItemData) => void;
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