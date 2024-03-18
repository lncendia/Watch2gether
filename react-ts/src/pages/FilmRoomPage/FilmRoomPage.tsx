import {useLocation} from "react-router-dom";
import {FilmRoomContextProvider} from "../../contexts/FilmRoomContext/FilmRoomContext.tsx";
import FilmRoomPlayerModule from "../../modules/FilmRoom/FilmRoomPlayerModule/FilmRoomPlayerModule.tsx";
import FilmRoomChatModule from "../../modules/FilmRoom/FilmRoomChatModule/FilmRoomChatModule.tsx";
import FilmRoomInfoModule from "../../modules/FilmRoom/FilmRoomInfoModule/FilmRoomInfoModule.tsx";
import {Col, Row} from "react-bootstrap";

const FilmRoomPage = () => {

    const {state} = useLocation();

    return (
        <FilmRoomContextProvider id={"f466e080-de02-44be-9535-60888c10db92"} url={"https://localhost:7291/"}>
            <Row>
                <Col xs={8}>
                    <FilmRoomPlayerModule/>
                </Col>
                <Col xs={4}>
                    <FilmRoomInfoModule/>
                    <FilmRoomChatModule/>
                </Col>
            </Row>
        </FilmRoomContextProvider>
    )
};

export default FilmRoomPage;