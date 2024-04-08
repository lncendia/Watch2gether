import {useNavigate} from "react-router-dom";
import {UserFilm} from "../../../services/ProfileService/ViewModels/ProfileViewModels.ts";
import FilmsList from "../../../components/Films/FilmsList/FilmsList.tsx";
import {FilmShortData} from "../../../components/Films/FilmShortItem/FilmShortData.ts";
import {useCallback} from "react";
import NoData from "../../../UI/NoData/NoData.tsx";

const UserFilmsModule = ({films, className}: { films: UserFilm[], className?: string }) => {

    // Навигационный хук
    const navigate = useNavigate();

    const onSelect = useCallback((film: FilmShortData) => {
        navigate('/film', {state: {id: film.id}})
    }, [navigate])

    if (films.length === 0) <NoData text="Пусто"/>

    return <FilmsList className={className} films={films} hasMore={false} next={() => {
    }} onSelect={onSelect}/>
};

export default UserFilmsModule;