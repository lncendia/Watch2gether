import {useInjection} from "inversify-react";
import {IFilmRoomService} from "../../../services/FilmRoomService/IFilmRoomService.ts";
import {useEffect, useState} from "react";
import {MessageData} from "../../../components/FilmRoom/Message/MessageData.ts";
import {Message} from "../../../services/FilmRoomService/Models/Messages.ts";
import InfiniteScroll from "react-infinite-scroll-component";
import Chat from "../../../components/FilmRoom/Chat/Chat.tsx";
import {useUser} from "../../../contexts/UserContext.tsx";
import Spinner from "../../../components/Common/Spinner/Spinner.tsx";
import SendMessageForm from "../SendMessageForm/SendMessageForm.tsx";


const FilmRoomChatModule = ({viewers}: { viewers: FilmViewer[] }) => {

    const [messages, setMessages] = useState<MessageData[]>([])
    const [lastMessageId, setLastMessageId] = useState<string>()
    const [hasMore, setHasMore] = useState(false)
    const roomService = useInjection<IFilmRoomService>('FilmRoomService');
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

    roomService.messages.attach((response) => {
        setMessages([...messages, ...response.messages.map(map)])
        setHasMore(response.count > response.messages.length)
        setLastMessageId(response.messages[response.messages.length - 1].id)
        console.log(response.count > response.messages.length)
    })

    useEffect(() => {
        roomService.getMessages().then()
    }, [])


    const onBottom = () => {
        roomService.getMessages(lastMessageId).then()
    }

    const sendMessage = async (text: string) => {
        await roomService.sendMessage(text)
        const viewer = viewers.filter(v => v.id === authorizedUser!.id)[0]
        let message: MessageData = {
            photoUrl: viewer.photoUrl ?? "/vite.png",
            text: text,
            createdAt: new Date(),
            userId: viewer.id,
            username: viewer.username
        }

        setMessages([message, ...messages])
    }

    const scrollProps = {
        dataLength: messages.length,
        next: onBottom,
        hasMore: hasMore,
        className: "mt-4",
        style: {display: 'flex', flexDirection: 'column-reverse'},
        inverse: true,
        scrollableTarget: "scrollableDiv"
    }


    return (
        <>
            <InfiniteScroll {...scrollProps}>
                <Chat userId={authorizedUser!.id} messages={messages}>
                    <SendMessageForm callback={sendMessage}/>
                </Chat>
            </InfiniteScroll>
        </>
    );
};

export default FilmRoomChatModule;