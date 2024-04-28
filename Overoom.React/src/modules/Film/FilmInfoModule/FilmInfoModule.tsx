import FilmInfo from "../../../components/Film/FilmInfo/FilmInfo.tsx";
import {useNavigate} from "react-router-dom";
import {useInjection} from "inversify-react";
import {IProfileService} from "../../../services/ProfileService/IProfileService.ts";
import {useUser} from "../../../contexts/UserContext/UserContext.tsx";
import {FilmInfoData} from "../../../components/Film/FilmInfo/FilmInfoData.ts";
import {useCallback, useState} from "react";
import {Film} from "../../../services/FilmsService/ViewModels/FilmViewModels.ts";

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
        ...film,
        type: type,
        actors: film.actors.map(c => [c.name, c.description]),
        countries: film.countries.map(c => [c, undefined]),
        description: film.description,
        directors: film.directors.map(c => [c, undefined]),
        genres: film.genres.map(c => [c, undefined]),
        screenWriters: film.screenWriters.map(c => [c, undefined]),
    }
}

interface FilmInfoProps {
    film: Film,
    className?: string,
    onRoomCreateClicked: () => void
}

const FilmInfoModule = ({film, className, onRoomCreateClicked}: FilmInfoProps) => {

    const [watchList, setWatchlist] = useState(film.inWatchlist ?? false)

    const profileService = useInjection<IProfileService>('ProfileService');
    const {authorizedUser} = useUser()

    // Навигационный хук
    const navigate = useNavigate();

    const toggleWatchlist = useCallback(async () => {
        if (authorizedUser === null) return
        await profileService.toggleWatchlist(film.id)
        setWatchlist(prev => !prev)
    }, [authorizedUser, profileService, film.id])

    return (
        <FilmInfo className={className} film={map(film)}
                  onCountrySelect={value => navigate('/filmSearch', {state: {country: value}})}
                  onGenreSelect={value => navigate('/filmSearch', {state: {genre: value}})}
                  onPersonSelect={value => navigate('/filmSearch', {state: {person: value}})}
                  onYearSelect={value => navigate('/filmSearch', {state: {year: value}})}
                  onTypeSelect={value => navigate('/filmSearch', {state: {serial: value === 'Сериал'}})}
                  isWatchlistEnabled={authorizedUser !== null} inWatchlist={watchList}
                  onWatchlistToggle={toggleWatchlist} onRoomCreateClicked={onRoomCreateClicked}/>
    );
};

export default FilmInfoModule;