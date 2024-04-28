import {MessageData} from "./MessageData.ts";
import moment from "moment/moment";
import styles from "./Message.module.css"
import RoomAvatar from "../../../../UI/RoomAvatar/RoomAvatar.tsx";

interface MessageProps {
    message: MessageData,
    className?: string,
    isMe: boolean,
    isOwner: boolean
}

const Message = ({message, className = '', isMe, isOwner}: MessageProps) => {

    if (isMe) return (
        <div className={`d-flex flex-row justify-content-start mb-4 ${className}`}>
            <div className={`${styles.message} ${styles.me}`}>
                <div className={styles.username}>{message.username}</div>
                <div className={styles.text}>{message.text}</div>
                <div className={styles.createdAt}>{moment(message.createdAt).format("HH:mm:ss")}</div>
            </div>
            <RoomAvatar owner={isOwner} src={message.photoUrl}/>
        </div>
    )

    return (
        <div className={`d-flex flex-row justify-content-end mb-4 ${className}`}>
            <RoomAvatar owner={isOwner} src={message.photoUrl}/>
            <div className={`${styles.message} ${styles.other}`}>
                <div className={styles.username}>{message.username}</div>
                <div className={styles.text}>{message.text}</div>
                <div className={styles.createdAt}>{moment(message.createdAt).format("HH:mm:ss")}</div>
            </div>
        </div>
    );
};


export default Message;