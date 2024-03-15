import {useNavigate} from "react-router-dom";
import {UserFilm} from "../../../services/ProfileService/Models/Profile.ts";
import FilmsList from "../../../components/Films/FilmsList/FilmsList.tsx";
import {FilmShortData} from "../../../components/Films/FilmShortItem/FilmShortData.ts";
import {Col} from "react-bootstrap";

const UserFilmsModule = ({films, className}: { films: UserFilm[], className?: string }) => {

    // Навигационный хук
    const navigate = useNavigate();

    const onFilmSelect = (film: FilmShortData) => {
        navigate('/film', {state: {id: film.filmId}})
    }

    if (films.length === 0) return <></>

    return (
        <Col xl={9} lg={10} className={className}>
            <FilmsList className={"mt-3"} films={films} onFilmSelect={onFilmSelect}/>
        </Col>
    );
};

export default UserFilmsModule;