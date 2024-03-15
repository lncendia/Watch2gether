import {MessageData} from "../Message/MessageData.ts";
import styles from "./Chat.module.css"
import Message from "../Message/Message.tsx";
import React from "react";

const Chat = ({messages, children, userId, className = ''}: {
    userId: string,
    messages: MessageData[],
    className?: string
    children: React.ReactNode
}) => {
    return (
        <div className={className}>
            <div className={styles.messages}>
                {messages.map(m => <Message key={m.createdAt.getTime()} message={m} userId={userId}/>)}
            </div>
            <div className={styles.send}>
                {children}
            </div>
        </div>
    );
};

export default Chat;