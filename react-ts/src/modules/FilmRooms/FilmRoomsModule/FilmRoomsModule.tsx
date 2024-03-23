import {useEffect, useState} from "react";
import {FilmItemData} from "../../../components/Films/FilmItem/FilmItemData.ts";
import {useInjection} from "inversify-react";
import {useNavigate} from "react-router-dom";
import Spinner from "../../../components/Common/Spinner/Spinner.tsx";
import InfiniteScroll from "react-infinite-scroll-component";
import FilmsCatalog from "../../../components/Films/FilmsCatalog/FilmsCatalog.tsx";
import {FilmRoom} from "../../../services/RoomsService/Models/Rooms.ts";
import {IFilmRoomsService} from "../../../services/RoomsService/IFilmRoomsService.ts";

interface FilmRoomsModuleProps {
    onlyPublic?: boolean;
    onlyMy?: boolean;
    page?: number;
    countPerPage?: number;
    filmId?: string;
    className?: string
}

const FilmRoomsModule = (props: FilmRoomsModuleProps) => {
    const [rooms, setRooms] = useState<FilmRoom[]>([]);
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

    const onBottom = () => {
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
    }

    const onFilmSelect = (film: FilmItemData) => {
        navigate('/film', {state: {id: film.id}})
    }

    const scrollProps = {
        dataLength: rooms.length,
        next: onBottom,
        hasMore: hasMore,
        loader: <Spinner/>,
        className: props.className
    }

    return (
        <InfiniteScroll {...scrollProps}>
            <FilmsCatalog genre={props.genre} films={films} onFilmSelect={onFilmSelect}
                          typeSelected={props.serial !== undefined}/>
        </InfiniteScroll>
    );
};

export default FilmRoomsModule;