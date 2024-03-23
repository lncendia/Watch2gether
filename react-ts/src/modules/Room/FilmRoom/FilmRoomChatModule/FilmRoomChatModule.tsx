import {useCallback, useEffect, useState} from "react";
import {MessageData} from "../../../../components/FilmRoom/Message/MessageData.ts";
import InfiniteScroll from "react-infinite-scroll-component";
import Chat from "../../../../components/FilmRoom/Chat/Chat.tsx";
import SendMessageForm from "../../Common/SendMessageForm/SendMessageForm.tsx";
import {useFilmRoom} from "../../../../contexts/FilmRoomContext/FilmRoomContext.tsx";
import Spinner from "../../../../components/Common/Spinner/Spinner.tsx";
import ConnectLinkModule from "../../Common/ConnectLinkModule/ConnectLinkModule.tsx";

export interface Message {
    userId: string;
    createdAt: Date;
    text: string;
}

const FilmRoomChatModule = () => {

    const [messages, setMessages] = useState<Message[]>([])
    const [lastMessageId, setLastMessageId] = useState<string>()
    const [hasMore, setHasMore] = useState(false)
    const {viewers, service, room} = useFilmRoom()

    const mapMessage = useCallback((m: Message): MessageData => {

        const viewer = viewers.filter(v => v.id === m.userId)[0]

        return {
            photoUrl: viewer.photoUrl ?? "/vite.png",
            text: m.text,
            createdAt: new Date(m.createdAt),
            userId: m.userId,
            username: viewer.username
        }
    }, [viewers])


    useEffect(() => {
        service.messagesEvent.attach((response) => {
            setMessages(messages => [...messages, ...response.messages])
            setHasMore(response.count > response.messages.length)
            setLastMessageId(response.messages[response.messages.length - 1].id)
        })

        service.messageEvent.attach((message) => {
            setMessages(prev => [message, ...prev])
        })

        service.getMessages().then()
    }, [service])


    const onBottom = useCallback(() => {
        service.getMessages(lastMessageId).then()
    }, [lastMessageId, service])

    const sendMessage = useCallback((async (text: string) => {
        await service.sendMessage(text)
    }), [service])

    const scrollProps = {
        dataLength: messages.length,
        next: onBottom,
        hasMore: hasMore,
        inverse: true,
        scrollableTarget: "scrollableDiv",
        loader: <Spinner/>
    }


    return (
        <>
            <InfiniteScroll style={{display: 'flex', flexDirection: 'column-reverse'}} {...scrollProps}>
                <Chat userId={room.currentId} messages={messages.map(mapMessage)} ownerId={room.ownerId}>
                    <SendMessageForm callback={sendMessage}/>
                    <ConnectLinkModule code={room.code} id={room.id} endpoint="filmRoom"/>
                </Chat>
            </InfiniteScroll>
        </>
    );
};

export default FilmRoomChatModule;