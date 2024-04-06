import Navbar from "../../../components/Menu/Navbar/Navbar.tsx";
import {useCallback, useState} from "react";
import {useInjection} from "inversify-react";
import {IFilmsService} from "../../../services/FilmsService/IFilmsService.ts";
import {useNavigate} from "react-router-dom";
import {IAuthService} from "../../../services/AuthService/IAuthService.ts";
import {useUser} from "../../../contexts/UserContext/UserContext.tsx";
import {FilmShort} from "../../../services/FilmsService/Models/Films.ts";

const NavbarModule = () => {

    const [films, setFilms] = useState<FilmShort[]>([]);
    const filmService = useInjection<IFilmsService>('FilmsService');
    const authService = useInjection<IAuthService>('AuthService');
    const {authorizedUser} = useUser()

    // Навигационный хук
    const navigate = useNavigate();

    const onFilmSearch = useCallback(async (value: string) => {
        if (value === '') setFilms([])
        else {
            const filmsResponse = await filmService.search({query: value})
            setFilms(filmsResponse.films)
        }
    }, [filmService])


    const onLogout = useCallback(authService.signOut.bind(authService), [authService])
    const onLogin = useCallback(authService.signIn.bind(authService), [authService])
    const onCatalog = useCallback(() => navigate('/catalog'), [navigate])
    const onPlaylists = useCallback(() => navigate('/playlists'), [navigate])
    const onRooms = useCallback(() => navigate('/filmRooms'), [navigate])
    const onYouTube = useCallback(() => navigate('/youtube'), [navigate])
    const onProfile = useCallback(() => navigate('/profile'), [navigate])
    const onHome = useCallback(() => navigate('/'), [navigate])

    const onFilm = useCallback((film: FilmShort) => {
        navigate('/film', {state: {id: film.id}})
    }, [navigate])

    return (
        <Navbar onFilm={onFilm} onHome={onHome} onCatalog={onCatalog} onLogin={onLogin} onLogout={onLogout}
                onPlaylists={onPlaylists} onRooms={onRooms} onProfile={onProfile} onYouTube={onYouTube} films={films}
                onFilmSearch={onFilmSearch} user={authorizedUser}/>
    );
};

export default NavbarModule;