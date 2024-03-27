import FilmRoomInfo from "../../../../components/Room/FilmRoom/FilmRoomInfo/FilmRoomInfo.tsx";
import {useFilmRoom} from "../../../../contexts/FilmRoomContext/FilmRoomContext.tsx";
import {useCallback} from "react";
import {useNavigate} from "react-router-dom";

const FilmRoomInfoModule = ({className}: { className?: string }) => {

    const {room, service} = useFilmRoom()
    const navigate = useNavigate()

    const leave = useCallback(async () => {
        await service.leave()
        navigate("/")
    }, [service, navigate])

    return <FilmRoomInfo film={room} className={className} onLeaveClicked={leave}/>
};

export default FilmRoomInfoModule;