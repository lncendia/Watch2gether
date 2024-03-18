import Carousel from "react-multi-carousel";
import FilmShortItem from "../FilmShortItem/FilmShortItem.tsx";
import {FilmShortData} from "../FilmShortItem/FilmShortData.ts";

const FilmsSlider = ({films, onFilmSelect, className}: {
    films: FilmShortData[],
    className?: string,
    onFilmSelect: (film: FilmShortData) => void
}) => {

    const responsive = {
        superLargeMonitor: {
            breakpoint: {max: 4000, min: 3000},
            items: 10,
            slidesToSlide: 1
        },
        monitor: {
            breakpoint: {max: 3000, min: 1700},
            items: 9,
            slidesToSlide: 1
        },
        desktop: {
            breakpoint: {max: 1700, min: 1150},
            items: 7,
            slidesToSlide: 1
        },
        notepad: {
            breakpoint: {max: 1150, min: 990},
            items: 5,
            slidesToSlide: 1
        },
        tablet: {
            breakpoint: {max: 990, min: 464},
            items: 3,
            slidesToSlide: 1
        },
        mobile: {
            breakpoint: {max: 464, min: 0},
            items: 2,
            slidesToSlide: 1
        }
    };

    const carouselProps = {
        infinite: true,
        autoPlay: true,
        autoPlaySpeed: 5000,
        keyBoardControl: true,
        responsive: responsive,
        className: className
    }


    return (
        <Carousel {...carouselProps}>
            {films.map(film => <FilmShortItem key={film.id} film={film} onClick={() => onFilmSelect(film)}/>)}
        </Carousel>
    );
};

export default FilmsSlider;