import {useLocation} from "react-router-dom";
import FilmRoomModule from "../../modules/FilmRoom/FilmRoomModule/FilmRoomModule.tsx";

const FilmRoomPage = () => {

    const {state} = useLocation();

    // return <FilmRoomModule id={state.id} url={state.url}/>
    return <FilmRoomModule id={"d65ebdda-c050-454a-b456-3533965cd807"} url={"https://localhost:7291/"}/>
};

export default FilmRoomPage;