import {useCallback, useEffect, useMemo, useState} from "react";
import Chat from "../../../../components/Room/Common/Chat/Chat.tsx";
import SendMessageForm from "../../Common/SendMessageForm/SendMessageForm.tsx";
import {useFilmRoom} from "../../../../contexts/FilmRoomContext/FilmRoomContext.tsx";
import ConnectLinkModule from "../../Common/ConnectLinkModule/ConnectLinkModule.tsx";
import {MessageData} from "../../../../components/Room/Common/Message/MessageData.ts";

export interface Message {
    userId: string;
    createdAt: Date;
    text: string;
}

const FilmRoomChatModule = () => {

    const [messages, setMessages] = useState<Message[]>([])
    const [lastMessageId, setLastMessageId] = useState<string>()
    const [hasMore, setHasMore] = useState(false)
    const {viewers, service, room, type} = useFilmRoom()

    const mappedMessages = useMemo<MessageData[]>(() => {
        return messages.map(m => {
            const viewer = viewers.filter(v => v.id === m.userId)[0]

            return {
                createdAt: new Date(m.createdAt),
                photoUrl: viewer?.photoUrl ?? "/img/profile.svg",
                text: m.text,
                userId: m.userId,
                username: viewer?.username ?? "Зритель"
            }
        })
    }, [viewers, messages])


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

    const onType = useCallback((async () => {
        await service.type()
        type(room.currentId)
    }), [service, room.currentId, type])

    return (
        <Chat next={onBottom} hasMore={hasMore} userId={room.currentId} messages={mappedMessages}
              ownerId={room.ownerId}>
            <SendMessageForm type={onType} callback={sendMessage}/>
            <ConnectLinkModule code={room.code} id={room.id} endpoint="filmRoom"/>
        </Chat>
    );
};

export default FilmRoomChatModule;