import {useLocation} from "react-router-dom";
import FilmRoomPlayerModule from "../../modules/Room/FilmRoom/FilmRoomPlayerModule/FilmRoomPlayerModule.tsx";
import FilmRoomChatModule from "../../modules/Room/FilmRoom/FilmRoomChatModule/FilmRoomChatModule.tsx";
import FilmRoomViewersModule from "../../modules/Room/FilmRoom/FilmRoomViewersModule/FilmRoomViewersModule.tsx";
import {Col, Row} from "react-bootstrap";
import ConnectFilmRoomModule from "../../modules/FilmRooms/ConnectFilmRoomModule/ConnectFilmRoomModule.tsx";
import BeepModule from "../../modules/Room/Common/BeepModule/BeepModule.tsx";
import ScreamModule from "../../modules/Room/Common/ScreamModule/ScreamModule.tsx";
import NotificationModule from "../../modules/Room/Common/NotificationModule/NotificationModule.tsx";
import FilmRoomRatingModule from "../../modules/Room/FilmRoom/FilmRoomRatingModule/FilmRoomRatingModule.tsx";
import FilmRoomInfoModule from "../../modules/Room/FilmRoom/FilmRoomInfoModule/FilmRoomInfoModule.tsx";
import AuthorizeModule from "../../modules/Authorization/AuthorizeModule.tsx";

const FilmRoomPage = () => {

    const location = useLocation();
    const params = new URLSearchParams(location.search);
    const code = params.get('code') ?? '';
    const id = params.get('id') ?? location.state.id;

    return (
        <AuthorizeModule showError={true}>
            <ConnectFilmRoomModule id={id} code={code}>
                <Row className="gy-4 mt-3">
                    <Col xl={8}>
                        <FilmRoomInfoModule/>
                        <FilmRoomPlayerModule className='mt-3'/>
                        <FilmRoomRatingModule className='mt-3'/>
                    </Col>
                    <Col xl={4}>
                        <FilmRoomViewersModule/>
                        <div className="position-relative">
                            <FilmRoomChatModule/>
                            <NotificationModule/>
                        </div>
                    </Col>
                </Row>
                <BeepModule/>
                <ScreamModule/>
            </ConnectFilmRoomModule>
        </AuthorizeModule>
    )
};

export default FilmRoomPage;