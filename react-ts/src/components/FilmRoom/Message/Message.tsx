import {MessageData} from "./MessageData.ts";
import moment from "moment/moment";
import styles from "./Message.module.css"
import RoomAvatar from "../../../UI/RoomAvatar/RoomAvatar.tsx";

interface MessageProps {
    message: MessageData,
    className?: string,
    isMe: boolean,
    isOwner: boolean
}

const Message = ({message, className = '', isMe, isOwner}: MessageProps) => {

    let justify: string
    let messageClass: string

    if (isMe) {
        justify = "justify-content-start"
        messageClass = styles.me
    } else {
        justify = "justify-content-end"
        messageClass = styles.other
    }

    return (
        <div className={`d-flex flex-row ${justify} mb-4 ${className}`}>
            <div className={`${styles.message} ${messageClass}`}>
                <div className={styles.username}>{message.username}</div>
                <div className={styles.text}>{message.text}</div>
                <div className={styles.createdAt}>{moment(message.createdAt).format("hh:mm:ss")}</div>
            </div>
            <RoomAvatar owner={isOwner} src={message.photoUrl}/>
        </div>
    );
};


export default Message;