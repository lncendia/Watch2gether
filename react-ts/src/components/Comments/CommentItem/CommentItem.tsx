import {Card} from "react-bootstrap";
import Svg from "../../../UI/Svg/Svg.tsx";
import styles from "./CommentItem.module.css"
import moment from 'moment'
import {CommentData} from "./CommentData.ts";

export interface CommentParams {
    comment: CommentData;
    removeComment: () => void;
    className?: string;
}

const CommentItem = (props: CommentParams) => {
    return (
        <Card className={`${props.className} ${styles.card}`}>
            <Card.Header>
                <span className="float-start">{props.comment.username}</span>
                <span className="float-end">{moment(props.comment.createdAt).format('DD.MM.YYYY HH:mm:ss')}</span>
            </Card.Header>
            <Card.Body className="d-flex">
                <img src={props.comment.avatarUrl} alt="" className={styles.avatar}/>
                <Card.Text>{props.comment.text}</Card.Text>
                {props.comment.isUserComment &&
                    <Svg onClick={props.removeComment} width={16} height={16} fill="currentColor"
                         className={`bi bi-trash-fill ${styles.remove}`} viewBox="0 0 16 16">
                        <path
                            d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 1 0z"/>
                    </Svg>}
            </Card.Body>
        </Card>
    );
};

export default CommentItem;