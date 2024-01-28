import React, {useEffect, useState} from 'react';
import {Film} from "../../../components/FilmItem/FilmItem";
import FilmList from "../../../components/FilmList/FilmList";
import InfiniteScroll from "react-infinite-scroll-component";
import Spinner from "../../../components/Spinner/Spinner";
import Rating from "../../../components/Rating/Rating";

// Вспомогательная функция для генерации случайного рейтинга от 1 до 10
const getRandomRating = (): number => parseFloat((Math.random() * 9 + 1).toFixed(1));

// Генерация уникального ID для фильма
const generateId = (): string => `id-${Math.random().toString(36).substr(2, 9)}`;

// Массив реальных названий жанров
const realGenres = ["Аниме", "Боевик", "Военный", "Детектив", "Драма", "Комедия", "Мелодрама", "Мультфильм", "Приключения", "Триллер", "Ужасы", "Фантастика", "Фэнтези"];

// Генерируем выдуманные названия фильмов
const movieTitles = [
    "Eternal Shadows",
    "Guardians of the Stars",
    "Whispers of the Past",
    "Infernal Heart",
    "Beyond the Horizon",
    "Echoes of Tomorrow",
    "The Last Sentinel",
    "Secrets of the Abyss",
    "The Architect's Dream",
    "Chains of Destiny",
    "The Mirage of Time",
    "Warriors of the Forgotten Realms",
    "Silent Betrayal",
    "The Enigma of Arrival",
    "Midnight Odyssey",
    "Ghosts of the Deep",
    "Crimson Dawn",
    "The Bard's Tale",
    "Agents of Illusion",
    "Legacy of the Ancients"
];

const pickRandomFilms = (arr: string[], n: number, genre: string): Film[] => {
    const shuffled = Array.from(arr).sort(() => 0.5 - Math.random());
    return shuffled
        .map(s => generateFilm(s, genre))
        .filter((f) => {
            if (genre === '') {
                return true; // Если входной жанр null, то возвращаем все фильмы
            } else {
                return f.genres.includes(genre as string); // Фильтруем фильмы по указанному жанру
            }
        })
        .slice(0, n);
};

const generateFilm = (title: string, genre: string): Film => {
    const randomGenreIndex = Math.floor(Math.random() * realGenres.length);
    let selectedGenres: string[] = [];
    for (let i = 0; i < 3; i++) {
        selectedGenres.push(realGenres[(randomGenreIndex + i) % realGenres.length]);
    }
    if (genre !== '' && !selectedGenres.includes(genre)) selectedGenres.push(genre)

    const id = generateId();

    return {
        name: title,
        description: `An intriguing story of ${title.toLowerCase()}, featuring unexpected turns and rich characters.`,
        genres: selectedGenres,
        type: "Фильм",
        posterUrl: `https://loremflickr.com/400/800/transport?id=${id}`,
        id: id,
        ratingKp: getRandomRating(),
        ratingImdb: getRandomRating(),
        ratingOveroom: getRandomRating()
    };
};

const FilmsCatalog = ({genre}: { genre: string }) => {

    const [films, setFilms] = useState<Film[]>(pickRandomFilms(movieTitles, 10, genre));

    useEffect(() => {
        setFilms(pickRandomFilms(movieTitles, 10, genre));
    }, [genre]); // Эффект будет вызываться при каждом изменении `genre`

    const onBottom = () => {
        setTimeout(() =>
                setFilms(films.concat(pickRandomFilms(movieTitles, 10, genre)))
            , 2000)
    }

    const scrollProps = {
        style: {overflow: "hidden"},
        dataLength: films.length,
        next: onBottom,
        hasMore: true,
        loader: <Spinner/>,
        endMessage: <p>No more data to load.</p>
    }

    return (
        <InfiniteScroll {...scrollProps}>
            <Rating disabled={true} scoreChanged={s => alert(s)}/>
            <FilmList films={films}/>
        </InfiniteScroll>
    );
};

export default FilmsCatalog;