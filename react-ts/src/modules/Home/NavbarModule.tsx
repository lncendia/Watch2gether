import Navbar from "../../components/Menu/Navbar/Navbar.tsx";
import {useState} from "react";
import {useInjection} from "inversify-react";
import {IFilmsService} from "../../services/FilmsService/IFilmsService.ts";
import {useNavigate} from "react-router-dom";
import {IAuthService} from "../../services/AuthService/IAuthService.ts";
import {useUser} from "../../contexts/UserContext.tsx";
import {FilmShort} from "../../services/FilmsService/Models/Films.ts";

const NavbarModule = () => {

    const [films, setFilms] = useState<FilmShort[]>([]);
    const filmService = useInjection<IFilmsService>('FilmsService');
    const authService = useInjection<IAuthService>('AuthService');
    const {authorizedUser} = useUser()

    // Навигационный хук
    const navigate = useNavigate();

    const onFilmSearch = async (value: string) => {
        if (value === '') setFilms([])
        else {
            const filmsResponse = await filmService.search({query: value})
            setFilms(filmsResponse.films)
        }
    }


    const onLogout = authService.signOut.bind(authService)
    const onLogin = authService.signIn.bind(authService)
    const onCatalog = () => navigate('/catalog')
    const onPlaylists = () => navigate('/playlists')
    const onRooms = () => navigate('/rooms')
    const onYouTube = () => navigate('/youtube')
    const onProfile = () => navigate('/profile')
    const onHome = () => navigate('/')

    const onFilm = (film: FilmShort) => {
        navigate('/film', {state: {id: film.id}})
    }

    return (
        <Navbar onFilm={onFilm} onHome={onHome} onCatalog={onCatalog} onLogin={onLogin} onLogout={onLogout}
                onPlaylists={onPlaylists} onRooms={onRooms} onProfile={onProfile} onYouTube={onYouTube} films={films}
                onFilmSearch={onFilmSearch} user={authorizedUser}/>
    );
};

export default NavbarModule;