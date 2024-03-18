import {useEffect, useState} from "react";
import {MessageData} from "../../../components/FilmRoom/Message/MessageData.ts";
import InfiniteScroll from "react-infinite-scroll-component";
import Chat from "../../../components/FilmRoom/Chat/Chat.tsx";
import {useUser} from "../../../contexts/UserContext/UserContext.tsx";
import SendMessageForm from "../SendMessageForm/SendMessageForm.tsx";
import {useFilmRoom} from "../../../contexts/FilmRoomContext/FilmRoomContext.tsx";
import Spinner from "../../../components/Common/Spinner/Spinner.tsx";


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
    const {authorizedUser} = useUser()


    const map = (m: Message): MessageData => {

        const viewer = viewers.filter(v => v.id === m.userId)[0]

        return {
            photoUrl: viewer.photoUrl ?? "/vite.png",
            text: m.text,
            createdAt: new Date(m.createdAt),
            userId: m.userId,
            username: viewer.username
        }
    }

    useEffect(() => {
        service.messages.attach((response) => {
            setMessages(messages => [...messages, ...response.messages])
            setHasMore(response.count > response.messages.length)
            setLastMessageId(response.messages[response.messages.length - 1].id)
        })
        service.getMessages().then()
    }, [service])


    const onBottom = () => {
        service.getMessages(lastMessageId).then()
    }

    const sendMessage = async (text: string) => {
        await service.sendMessage(text)
        const viewer = viewers.filter(v => v.id === authorizedUser!.id)[0]
        const message: Message = {
            text: text,
            createdAt: new Date(),
            userId: viewer.id,
        }

        setMessages([message, ...messages])
    }

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
                <Chat userId={authorizedUser!.id} messages={messages.map(map)} ownerId={room.ownerId}>
                    <SendMessageForm callback={sendMessage}/>
                </Chat>
            </InfiniteScroll>
        </>
    );
};

export default FilmRoomChatModule;