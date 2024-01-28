import React, {useState} from 'react';
import Genre from "../../components/Genre/Genre";
import Carousel from "react-multi-carousel";
import "react-multi-carousel/lib/styles.css";


const genres = ["Аниме", "Боевик", "Военный", "Детектив", "Драма", "Комедия", "Мелодрама", "Мультфильм", "Приключения", "Триллер", "Ужасы", "Фантастика", "Фэнтези"]

const GenreSelect = ({className, genreSelected}: { className?: string, genreSelected: (genre: string) => void }) => {

    const [selectedGenre, selectGenre] = useState("")

    const responsive = {
        superLargeMonitor: {
            breakpoint: {max: 4000, min: 3000},
            items: 6,
            slidesToSlide: 1
        },
        monitor: {
            breakpoint: {max: 3000, min: 1500},
            items: 5,
            slidesToSlide: 1
        },
        desktop: {
            breakpoint: {max: 1500, min: 1024},
            items: 4,
            slidesToSlide: 1
        },
        tablet: {
            breakpoint: {max: 1024, min: 464},
            items: 2,
            slidesToSlide: 1
        },
        mobile: {
            breakpoint: {max: 464, min: 0},
            items: 1,
            slidesToSlide: 1
        }
    };

    const carouselProps = {
        infinite: true,
        autoPlay: selectedGenre === "",
        autoPlaySpeed: 5000,
        keyBoardControl: true,
        className: className,
        responsive: responsive
    }

    const toggleGenre = (genre: string) => {

        let genreValue = genre === selectedGenre ? '' : genre
        selectGenre(genreValue)
        genreSelected(genreValue)
    }

    return (
        <Carousel {...carouselProps}>
            {genres.map(g => <Genre key={g} genre={g} selected={selectedGenre === g} onSelect={() => toggleGenre(g)}/>)}
        </Carousel>
    );
};

export default GenreSelect;