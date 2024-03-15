import FilmInfo from "../../../components/Film/FilmInfo/FilmInfo.tsx";
import {Film} from "../../../services/FilmsService/Models/Film.ts";
import {useNavigate} from "react-router-dom";
import {useInjection} from "inversify-react";
import {IProfileService} from "../../../services/ProfileService/IProfileService.ts";
import {useUser} from "../../../contexts/UserContext.tsx";
import {FilmInfoData} from "../../../components/Film/FilmInfo/FilmInfoData.ts";
import {useState} from "react";
import CreateFilmRoomForm from "../CreateFilmRoomForm/CreateFilmRoomForm.tsx";
import {IRoomsService} from "../../../services/RoomsService/IRoomsService.ts";

const getSeasonsString = (count: number) => {
    count = count % 10
    if (count === 1) return 'сезон'
    if (count > 1 && count < 5) return 'сезона'
    return 'сезонов'
}

const getEpisodesString = (count: number) => {
    count = count % 10
    if (count === 1) return 'эпизод'
    if (count > 1 && count < 5) return 'эпизода'
    return 'эпизодов'
}


const map = (film: Film): FilmInfoData => {

    let type: [string, string?]

    if (film.isSerial) {
        type = ["Сериал", `${film.countSeasons} ${getSeasonsString(film.countSeasons!)}, ${film.countEpisodes} ${getEpisodesString(film.countEpisodes!)}`]
    } else {
        type = ["Фильм", undefined]
    }

    return {
        type: type,
        actors: film.actors.map(c => [c.name, c.description]),
        countries: film.countries.map(c => [c, undefined]),
        description: film.description,
        directors: film.directors.map(c => [c, undefined]),
        genres: film.genres.map(c => [c, undefined]),
        screenWriters: film.screenWriters.map(c => [c, undefined]),
        id: film.id,
        posterUrl: film.posterUrl,
        ratingImdb: film.ratingImdb,
        ratingKp: film.ratingKp,
        title: film.title,
        year: film.year

    }
}

const FilmInfoModule = ({film, className}: { film: Film, className?: string }) => {

    const [watchList, setWatchlist] = useState(film.inWatchlist ?? false)
    const [formOpen, setFormOpen] = useState(false)

    const profileService = useInjection<IProfileService>('ProfileService');
    const roomsService = useInjection<IRoomsService>('RoomsService');
    const {authorizedUser} = useUser()

    // Навигационный хук
    const navigate = useNavigate();

    const toggleWatchlist = async () => {
        if (authorizedUser === null) return
        await profileService.toggleWatchlist(film.id)
        setWatchlist(!watchList)
    }

    const createRoom = async (cdn: string, open: boolean) => {
        if (authorizedUser === null) return
        const response = await roomsService.createFilmRoom({
            open: open,
            filmId: film.id,
            cdnName: cdn
        })

        navigate("/filmRoom", {state: response})
    }

    return (
        <>
            <CreateFilmRoomForm open={formOpen} onClose={() => setFormOpen(false)} cdnList={film.cdnList}
                                callback={createRoom}/>
            <FilmInfo className={className} film={map(film)}
                      onCountrySelect={value => navigate('/search', {state: {country: value}})}
                      onGenreSelect={value => navigate('/search', {state: {genre: value}})}
                      onPersonSelect={value => navigate('/search', {state: {person: value}})}
                      onYearSelect={value => navigate('/search', {state: {year: value}})}
                      onTypeSelect={value => navigate('/search', {state: {serial: value === 'Сериал'}})}
                      isWatchlistEnabled={authorizedUser !== null} inWatchlist={watchList}
                      onWatchlistToggle={toggleWatchlist} onRoomCreateClicked={() => setFormOpen(true)}/>
        </>
    );
};

export default FilmInfoModule;