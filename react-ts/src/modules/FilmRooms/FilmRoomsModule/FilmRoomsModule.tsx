import {useCallback, useEffect, useState} from "react";
import {useInjection} from "inversify-react";
import {useNavigate} from "react-router-dom";
import {FilmRoomShort} from "../../../services/RoomsService/Models/Rooms.ts";
import {IFilmRoomsService} from "../../../services/RoomsService/IFilmRoomsService.ts";
import {FilmRoomItemData} from "../../../components/FilmRooms/FilmRoomItem/FilmRoomItemData.ts";
import FilmRoomsCatalog from "../../../components/FilmRooms/FilmRoomsCatalog/FilmRoomsCatalog.tsx";
import NoData from "../../../UI/NoData/NoData.tsx";

interface FilmRoomsModuleProps {
    onlyPublic?: boolean;
    onlyMy?: boolean;
    page?: number;
    countPerPage?: number;
    filmId?: string;
    className?: string
}

const FilmRoomsModule = (props: FilmRoomsModuleProps) => {
    const [rooms, setRooms] = useState<FilmRoomShort[]>([]);
    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(false);
    const roomsService = useInjection<IFilmRoomsService>('FilmRoomsService');

    // Навигационный хук
    const navigate = useNavigate();


    useEffect(() => {
        const processRooms = async () => {
            const response = await roomsService.search(props)
            setPage(2);
            setHasMore(response.countPages > 1)
            setRooms(response.rooms)
        };

        processRooms().then()
    }, [props, roomsService]);

    const next = useCallback(() => {
        const processRooms = async () => {
            const response = await roomsService.search({
                ...props,
                page: page
            })
            setPage(page + 1);
            setHasMore(response.countPages !== page)
            setRooms([...rooms, ...response.rooms])
        };

        processRooms().then()
    }, [props, page, rooms])

    const onSelect = useCallback((room: FilmRoomItemData) => {
        navigate('/filmRoom', {state: {id: room.id}})
    }, [navigate])

    if (rooms.length === 0) return <NoData className="mt-5" text="Подборки не найдены"/>

    return <FilmRoomsCatalog rooms={rooms} onSelect={onSelect} next={next} hasMore={hasMore}/>
};

export default FilmRoomsModule;