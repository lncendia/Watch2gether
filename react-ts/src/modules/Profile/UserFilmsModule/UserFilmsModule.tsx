import {useNavigate} from "react-router-dom";
import {UserFilm} from "../../../services/ProfileService/Models/Profile.ts";
import FilmsList from "../../../components/Films/FilmsList/FilmsList.tsx";
import {FilmShortData} from "../../../components/Films/FilmShortItem/FilmShortData.ts";
import {Col} from "react-bootstrap";
import {useCallback} from "react";
import NoData from "../../../UI/NoData/NoData.tsx";

const UserFilmsModule = ({films, className}: { films: UserFilm[], className?: string }) => {

    // Навигационный хук
    const navigate = useNavigate();

    const onSelect = useCallback((film: FilmShortData) => {
        navigate('/film', {state: {id: film.id}})
    }, [navigate])

    return (
        <Col xl={9} lg={10} className={className}>
            {films.length === 0 && <NoData text="Пусто"/>}
            {films.length !== 0 && <FilmsList films={films} hasMore={false} next={() => {
            }} onSelect={onSelect}/>}
        </Col>
    );
};

export default UserFilmsModule;