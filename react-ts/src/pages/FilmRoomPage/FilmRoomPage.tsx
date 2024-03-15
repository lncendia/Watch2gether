import {useLocation} from "react-router-dom";
import FilmRoomModule from "../../modules/FilmRoom/FilmRoomModule/FilmRoomModule.tsx";

const FilmRoomPage = () => {

    const {state} = useLocation();

    return <FilmRoomModule id={state.id} url={state.url}/>
};

export default FilmRoomPage;