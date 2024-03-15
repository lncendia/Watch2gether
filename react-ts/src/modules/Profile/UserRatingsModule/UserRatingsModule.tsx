import {Col} from "react-bootstrap";
import FilmsList from "../../../components/Films/FilmsList/FilmsList.tsx";
import {useNavigate} from "react-router-dom";
import {useInjection} from "inversify-react";
import {useEffect, useState} from "react";
import Spinner from "../../../components/Common/Spinner/Spinner.tsx";
import InfiniteScroll from "react-infinite-scroll-component";
import {IProfileService} from "../../../services/ProfileService/IProfileService.ts";
import {FilmShortData} from "../../../components/Films/FilmShortItem/FilmShortData.ts";

const UserRatingsModule = ({className}: { className?: string }) => {

    const [ratings, setRatings] = useState<FilmShortData[]>([]);
    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(false);

    const profileService = useInjection<IProfileService>('ProfileService');

    // Навигационный хук
    const navigate = useNavigate();


    useEffect(() => {
        const processRatings = async () => {
            const response = await profileService.ratings({})

            setPage(2);
            setHasMore(response.countPages > 1)
            setRatings(response.ratings)
        };

        processRatings().then()
    }, []);

    const onBottom = () => {
        const processRatings = async () => {
            const response = await profileService.ratings({
                page: page
            })
            setPage(page + 1);
            setHasMore(response.countPages !== page)
            setRatings([...ratings, ...response.ratings])
        };

        processRatings().then()
    }

    const onFilmSelect = (film: FilmShortData) => {
        navigate('/film', {state: {id: film.filmId}})
    }

    const scrollProps = {
        dataLength: ratings.length,
        next: onBottom,
        hasMore: hasMore,
        loader: <Spinner/>,
        className: "mt-3"
    }

    return (
        <Col xl={9} lg={10} className={className}>
            <InfiniteScroll {...scrollProps}>
                <FilmsList films={ratings} onFilmSelect={onFilmSelect}/>
            </InfiniteScroll>
        </Col>
    );
};

export default UserRatingsModule;