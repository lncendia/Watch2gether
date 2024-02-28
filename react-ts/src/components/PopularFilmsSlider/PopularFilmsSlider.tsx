import Carousel from "react-multi-carousel";
import {FilmShort} from "../../services/FilmsService/Models/FilmShort.ts";
import PopularFilmSliderItem from "./PopularFilmSliderItem.tsx";

const PopularFilmsSlider = ({films}: {
    films: FilmShort[]
}) => {

    const responsive = {
        superLargeMonitor: {
            breakpoint: {max: 4000, min: 3000},
            items: 7,
            slidesToSlide: 1
        },
        monitor: {
            breakpoint: {max: 3000, min: 1500},
            items: 6,
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
            items: 2,
            slidesToSlide: 1
        }
    };

    const carouselProps = {
        infinite: true,
        autoPlay: true,
        autoPlaySpeed: 5000,
        keyBoardControl: true,
        responsive: responsive
    }


    return (
        <Carousel {...carouselProps}>
            {films.map(film => <PopularFilmSliderItem key={film.id} film={film}/>)}
        </Carousel>
    );
};

export default PopularFilmsSlider;