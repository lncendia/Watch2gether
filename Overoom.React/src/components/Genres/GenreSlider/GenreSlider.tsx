import Carousel from "react-multi-carousel";
import GenreSliderItem from "../GenreSliderItem/GenreSliderItem.tsx";

const GenreSlider = ({genre, genres, className, genreSelected}: {
    genre: string | undefined
    className?: string
    genreSelected: (genre?: string) => void,
    genres: string[]
}) => {

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
            items: 2,
            slidesToSlide: 1
        }
    };

    const carouselProps = {
        infinite: true,
        autoPlay: genre === undefined,
        autoPlaySpeed: 5000,
        keyBoardControl: true,
        className: className,
        responsive: responsive,
        sliderClass: ""
    }

    const toggleGenre = (newGenre: string) => {

        if (genre === newGenre) genreSelected(undefined)
        else genreSelected(newGenre)
    }

    return (
        <Carousel {...carouselProps}>
            {genres.map(g => <GenreSliderItem key={g} genre={g} selected={genre === g}
                                          onSelect={() => toggleGenre(g)}/>)}
        </Carousel>
    );
};

export default GenreSlider;