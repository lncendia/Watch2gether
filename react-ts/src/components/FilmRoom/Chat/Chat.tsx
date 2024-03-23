import {MessageData} from "../Message/MessageData.ts";
import styles from "./Chat.module.css"
import Message from "../Message/Message.tsx";
import React from "react";

interface ChatProps {
    userId: string,
    messages: MessageData[],
    className?: string
    ownerId: string,
    children: React.ReactNode
}

const Chat = (props: ChatProps) => {
    return (
        <div className={props.className}>
            <div className={styles.messages}>
                {props.messages.map(m =>
                    (
                        <Message key={m.createdAt.getTime()} message={m} isMe={m.userId === props.userId}
                                 isOwner={m.userId === props.ownerId}/>
                    )
                )}
            </div>
            <div className={styles.send}>
                {props.children}
            </div>
        </div>
    );
};

export default Chat;