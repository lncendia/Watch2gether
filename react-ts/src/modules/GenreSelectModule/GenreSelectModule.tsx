import "react-multi-carousel/lib/styles.css";
import GenreSlider from "../../components/GenreSlider/GenreSlider.tsx";


const genres = ["Аниме", "Боевик", "Военный", "Детектив", "Драма", "Комедия", "Мелодрама", "Мультфильм", "Приключения", "Триллер", "Ужасы", "Фантастика", "Фэнтези"]

const GenreSelectModule = ({genre, className, genreSelected}: {
    genre: string | undefined
    className?: string
    genreSelected: (genre?: string) => void
}) => {

    return (
        <div className={className}>
            <h3 className={"mb-3"}>Жанры</h3>
            <GenreSlider genre={genre} genreSelected={genreSelected} genres={genres}/>
        </div>
    );
};

export default GenreSelectModule;