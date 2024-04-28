import FilmRatingModule from "../../../Film/FilmRatingModule/FilmRatingModule.tsx";
import {useFilmRoom} from "../../../../contexts/FilmRoomContext/FilmRoomContext.tsx";

const FilmRoomRatingModule = ({className}: { className?: string }) => {
    const {room} = useFilmRoom()

    return <FilmRatingModule className={className} id={room.filmId} userRating={room.userRating}
                             userRatingsCount={room.userRatingsCount} isSerial={room.isSerial}
                             userScore={room.userScore}/>
};

export default FilmRoomRatingModule;