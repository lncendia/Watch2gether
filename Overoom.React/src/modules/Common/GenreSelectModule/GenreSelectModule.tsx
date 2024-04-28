import "react-multi-carousel/lib/styles.css";
import GenreSlider from "../../../components/Genres/GenreSlider/GenreSlider.tsx";

const genres = ["Аниме", "Боевик", "Военный", "Детектив", "Драма", "Комедия", "Мелодрама", "Мультфильм", "Приключения", "Триллер", "Ужасы", "Фантастика", "Фэнтези"]

const GenreSelectModule = ({genre, className, genreSelected}: {
    genre: string | undefined
    className?: string
    genreSelected: (genre?: string) => void
}) => {

    return <GenreSlider className={className} genre={genre} genreSelected={genreSelected} genres={genres}/>

};

export default GenreSelectModule;