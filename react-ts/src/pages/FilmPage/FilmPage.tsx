import FilmModule from "../../modules/Film/FilmModule/FilmModule.tsx";
import {useLocation} from "react-router-dom";
import FilmCommentsModule from "../../modules/Film/FilmCommentsModule/FilmCommentsModule.tsx";

const FilmPage = () => {

    const {state} = useLocation();

    return (
        <div>
            <FilmModule className="mt-3" id={state.id}/>
            <FilmCommentsModule className="mt-5" id={state.id}/>
        </div>
    );
};

export default FilmPage;