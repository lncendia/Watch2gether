import BlockTitle from "../../UI/BlockTitle/BlockTitle.tsx";
import FilmRoomsModule from "../../modules/FilmRooms/FilmRoomsModule/FilmRoomsModule.tsx";
import {useUser} from "../../contexts/UserContext/UserContext.tsx";
import UserFilmRoomsModule from "../../modules/FilmRooms/UserFilmRoomsModule/UserFilmRoomsModule.tsx";

const FilmRoomsPage = () => {

    const {authorizedUser} = useUser()

    return (
        <>
            {authorizedUser &&
                <>
                    <BlockTitle title="Ваши комнаты" className="mt-3"/>
                    <UserFilmRoomsModule className="mb-5"/>
                </>
            }
            <FilmRoomsModule/>
        </>
    );
};

export default FilmRoomsPage;