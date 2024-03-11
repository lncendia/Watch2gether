import {useNavigate} from "react-router-dom";
import {UserFilm} from "../../../services/ProfileService/Models/Profile.ts";
import BlockTitle from "../../../UI/BlockTitle/BlockTitle.tsx";
import FilmsList from "../../../components/Films/FilmsList/FilmsList.tsx";
import {FilmShortData} from "../../../components/Films/FilmShortItem/FilmShortData.ts";
import {Col} from "react-bootstrap";

const UserFilmsModule = ({films, className, title}: { films: UserFilm[], className?: string, title: string }) => {

    // Навигационный хук
    const navigate = useNavigate();

    const onFilmSelect = (film: FilmShortData) => {
        navigate('/film', {state: {id: film.filmId}})
    }

    if (films.length === 0) return <></>

    return (
        <Col xl={9} lg={10} className={className}>
            <BlockTitle title={title}/>
            <FilmsList className={"mt-3"} films={films} onFilmSelect={onFilmSelect}/>
        </Col>
    );
};

export default UserFilmsModule;