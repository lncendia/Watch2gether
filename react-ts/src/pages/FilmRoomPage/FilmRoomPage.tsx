import {useLocation} from "react-router-dom";
import FilmRoomPlayerModule from "../../modules/Room/FilmRoom/FilmRoomPlayerModule/FilmRoomPlayerModule.tsx";
import FilmRoomChatModule from "../../modules/Room/FilmRoom/FilmRoomChatModule/FilmRoomChatModule.tsx";
import FilmRoomInfoModule from "../../modules/Room/FilmRoom/FilmRoomInfoModule/FilmRoomInfoModule.tsx";
import {Col, Row} from "react-bootstrap";
import ConnectFilmRoomModule from "../../modules/Room/FilmRoom/ConnectFilmRoomModule/ConnectFilmRoomModule.tsx";
import BeepModule from "../../modules/Room/Common/BeepModule/BeepModule.tsx";
import ScreamModule from "../../modules/Room/Common/ScreamModule/ScreamModule.tsx";
import NotificationModule from "../../modules/Room/Common/NotificationModule/NotificationModule.tsx";

const FilmRoomPage = () => {

    const location = useLocation();
    const params = new URLSearchParams(location.search);
    const code = params.get('code') ?? '';

    return (
        <ConnectFilmRoomModule id={"2aad6f12-b1c6-482e-89f1-cf89c7ad8d05"} code={code}>
            <Row className="gy-4">
                <Col xl={8}>
                    <FilmRoomPlayerModule/>
                </Col>
                <Col xl={4}>
                    <FilmRoomInfoModule/>
                    <div className="position-relative">
                        <FilmRoomChatModule/>
                        <NotificationModule/>
                    </div>
                </Col>
            </Row>
            <BeepModule/>
            <ScreamModule/>
        </ConnectFilmRoomModule>
    )
};

export default FilmRoomPage;