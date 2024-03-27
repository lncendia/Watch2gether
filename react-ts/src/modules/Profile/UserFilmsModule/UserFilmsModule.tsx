import {useNavigate} from "react-router-dom";
import {UserFilm} from "../../../services/ProfileService/Models/Profile.ts";
import FilmsList from "../../../components/Films/FilmsList/FilmsList.tsx";
import {FilmShortData} from "../../../components/Films/FilmShortItem/FilmShortData.ts";
import {Col} from "react-bootstrap";
import {useCallback} from "react";

const UserFilmsModule = ({films, className}: { films: UserFilm[], className?: string }) => {

    // Навигационный хук
    const navigate = useNavigate();

    const onSelect = useCallback((film: FilmShortData) => {
        navigate('/film', {state: {id: film.id}})
    }, [navigate])

    if (films.length === 0) return <></>

    return (
        <Col xl={9} lg={10} className={className}>
            <FilmsList films={films} hasMore={false} next={() => {}} onSelect={onSelect}/>
        </Col>
    );
};

export default UserFilmsModule;