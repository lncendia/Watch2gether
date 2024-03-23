import {MessageData} from "../Message/MessageData.ts";
import styles from "./Chat.module.css"
import Message from "../Message/Message.tsx";
import React from "react";
import {BadgeData} from "../Badge/BadgeData.ts";
import Badge from "../Badge/Badge.tsx";

interface ChatProps {
    userId: string,
    messages: MessageData[],
    className?: string
    ownerId: string,
    badge?: BadgeData
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
            {props.badge && <Badge color={props.badge.color} text={props.badge.text}/>}
        </div>
    );
};

export default Chat;