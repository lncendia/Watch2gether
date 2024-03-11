import {useEffect, useState} from "react";
import {useInjection} from "inversify-react";
import {IFilmsService} from "../../../services/FilmsService/IFilmsService.ts";
import {Film} from "../../../services/FilmsService/Models/Film.ts";
import {useUser} from "../../../contexts/UserContext.tsx";
import Loader from "../../../UI/Loader/Loader.tsx";
import {IProfileService} from "../../../services/ProfileService/IProfileService.ts";
import FilmInfoModule from "../FilmInfoModule/FilmInfoModule.tsx";
import FilmPlayerModule from "../FilmPlayerModule/FilmPlayerModule.tsx";
import FilmRatingModule from "../FilmRatingModule/FilmRatingModule.tsx";

const FilmModule = ({id, className}: { id: string, className?: string }) => {

    const [film, setFilm] = useState<Film>();
    const filmService = useInjection<IFilmsService>('FilmsService');
    const profileService = useInjection<IProfileService>('ProfileService');
    const {authorizedUser} = useUser()

    useEffect(() => {
        const processFilm = async () => {
            const response = await filmService.get(id)
            setFilm(response)
        };

        processFilm().then(() => {
            if (authorizedUser) profileService.addToHistory(id).then()
        })
    }, [id]);

    if (!film) return <Loader/>

    return (
        <div className={className}>
            <FilmInfoModule className="mt-2" film={film}/>
            <FilmPlayerModule className="mt-5" film={film}/>
            <FilmRatingModule className="mt-3" film={film}/>
        </div>
    );
};

export default FilmModule;