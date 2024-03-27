import {MessageData} from "../Message/MessageData.ts";
import styles from "./Chat.module.css"
import Message from "../Message/Message.tsx";
import React from "react";
import InfiniteScroll from "react-infinite-scroll-component";
import Spinner from "../../../Common/Spinner/Spinner.tsx";

interface ChatProps {
    userId: string,
    messages: MessageData[],
    className?: string
    ownerId: string,
    children: React.ReactNode
    hasMore: boolean,
    next: () => void
}

const Chat = (props: ChatProps) => {

    const scrollProps = {
        dataLength: props.messages.length,
        next: props.next,
        hasMore: props.hasMore,
        inverse: true,
        scrollableTarget: "scrollableDiv",
        loader: <Spinner/>
    }

    return (
        <div className={`${props.className}`}>
            <InfiniteScroll height="70vh" className={styles.messages}
                            style={{display: 'flex', flexDirection: 'column-reverse'}} {...scrollProps}>
                {props.messages.map(m =>
                    (
                        <Message key={m.createdAt.getTime()} message={m} isMe={m.userId === props.userId}
                                 isOwner={m.userId === props.ownerId}/>
                    )
                )}
            </InfiniteScroll>
            <div className={styles.send}>
                {props.children}
            </div>
        </div>
    );
};

export default Chat;