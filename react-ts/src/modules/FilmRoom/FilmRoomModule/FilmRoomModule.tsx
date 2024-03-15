import FilmRoomPlayerModule from "../FilmRoomPlayerModule/FilmRoomPlayerModule.tsx";
import {useInjection} from "inversify-react";
import {IFilmRoomService} from "../../../services/FilmRoomService/IFilmRoomService.ts";
import {useEffect, useState} from "react";
import Loader from "../../../UI/Loader/Loader.tsx";
import {Col, Row} from "react-bootstrap";
import FilmRoomChatModule from "../FilmRoomChatModule/FilmRoomChatModule.tsx";

const FilmRoomModule = ({id, url}: { id: string, url: string }) => {

    const [room, setRoom] = useState<FilmRoom>()

    const roomService = useInjection<IFilmRoomService>('FilmRoomService');

    useEffect(() => {
        roomService.roomLoad.attach((room) => setRoom(room))
        roomService.connect(id, url).then()
    }, []);

    if (!room) return <Loader/>

    return (
        <Row>
            <Col xs={8}>
                <FilmRoomPlayerModule room={room}/>
            </Col>
            <Col xs={4}>
                <FilmRoomChatModule viewers={room.viewers}/>
            </Col>
        </Row>
    );
};

export default FilmRoomModule;