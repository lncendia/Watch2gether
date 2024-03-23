import {useLocation} from "react-router-dom";
import FilmCommentsModule from "../../modules/Film/FilmCommentsModule/FilmCommentsModule.tsx";
import {useEffect, useState} from "react";
import {Film} from "../../services/FilmsService/Models/Film.ts";
import {useInjection} from "inversify-react";
import {IFilmsService} from "../../services/FilmsService/IFilmsService.ts";
import {IProfileService} from "../../services/ProfileService/IProfileService.ts";
import {useUser} from "../../contexts/UserContext/UserContext.tsx";
import Loader from "../../UI/Loader/Loader.tsx";
import FilmInfoModule from "../../modules/Film/FilmInfoModule/FilmInfoModule.tsx";
import FilmPlayerModule from "../../modules/Film/FilmPlayerModule/FilmPlayerModule.tsx";
import FilmRatingModule from "../../modules/Film/FilmRatingModule/FilmRatingModule.tsx";
import CreateFilmRoomModule from "../../modules/FilmRooms/CreateFilmRoomModule/CreateFilmRoomModule.tsx";

const FilmPage = () => {

    const {state} = useLocation();

    const [film, setFilm] = useState<Film>();
    const [formOpen, setFormOpen] = useState(false)
    const filmService = useInjection<IFilmsService>('FilmsService');
    const profileService = useInjection<IProfileService>('ProfileService');
    const {authorizedUser} = useUser()

    useEffect(() => {
        const processFilm = async () => {
            const response = await filmService.get(state.id)
            setFilm(response)
        };

        processFilm().then(() => {
            if (authorizedUser) profileService.addToHistory(state.id).then()
        })
    }, [authorizedUser, filmService, state, profileService]);

    if (!film) return <Loader/>

    return (
        <>
            <FilmInfoModule className="mt-3" film={film} onRoomCreateClicked={() => setFormOpen(true)}/>
            <FilmPlayerModule className="mt-5" film={film}/>
            <FilmRatingModule className="mt-3" film={film}/>
            <CreateFilmRoomModule cdnList={film.cdnList.map(c => [c.cdn, c.quality])} id={film.id}
                                  onClose={() => setFormOpen(false)} open={formOpen}/>
            <FilmCommentsModule className="mt-5" id={film.id}/>
        </>
    );
}

export default FilmPage;