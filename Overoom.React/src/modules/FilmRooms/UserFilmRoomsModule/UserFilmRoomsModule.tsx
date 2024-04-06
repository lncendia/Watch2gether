import {useCallback, useEffect, useState} from "react";
import {useInjection} from "inversify-react";
import {useNavigate} from "react-router-dom";
import {FilmRoomShort} from "../../../services/RoomsService/Models/Rooms.ts";
import {IFilmRoomsService} from "../../../services/RoomsService/IFilmRoomsService.ts";
import {FilmRoomItemData} from "../../../components/FilmRooms/FilmRoomItem/FilmRoomItemData.ts";
import NoData from "../../../UI/NoData/NoData.tsx";
import FilmsList from "../../../components/Films/FilmsList/FilmsList.tsx";


const UserFilmRoomsModule = ({className}: { className?: string }) => {
    const [rooms, setRooms] = useState<FilmRoomShort[]>([]);
    const roomsService = useInjection<IFilmRoomsService>('FilmRoomsService');

    // Навигационный хук
    const navigate = useNavigate();

    useEffect(() => {
        roomsService.my().then(r => setRooms(r))
    }, [roomsService]);

    const onSelect = useCallback((room: FilmRoomItemData) => {
        navigate('/filmRoom', {state: {id: room.id}})
    }, [navigate])

    if (rooms.length === 0) return <NoData className={className} text="У вас нет комнат"/>

    return <FilmsList className={className} films={rooms} onSelect={onSelect} next={() => {
    }} hasMore={false}/>
};

export default UserFilmRoomsModule;