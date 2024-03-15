import {MessageData} from "./MessageData.ts";
import moment from "moment/moment";
import styles from "./Message.module.css"

interface MessageProps {
    message: MessageData,
    className?: string,
    userId: string,
}

const Message = ({message, className = '', userId}: MessageProps) => {

    let justify: string
    let messageClass: string

    if (message.userId == userId) {
        justify = "justify-content-start"
        messageClass = styles.me
    } else {
        justify = "justify-content-end"
        messageClass = styles.other
    }

    return (
        <div className={`d-flex flex-row ${justify} mb-4 ${className}`}>
            <div className={`${styles.message} ${messageClass}`}>
                <p className={styles.username}>{message.username}</p>
                <p className="small text-break mb-0">{message.text}</p>
                <p className={styles.createdAt}>{moment(message.createdAt).format("hh:mm:ss")}</p>
            </div>
            <img src={message.photoUrl} className={styles.avatar} alt="Avatar"/>
        </div>
    );
};


export default Message;