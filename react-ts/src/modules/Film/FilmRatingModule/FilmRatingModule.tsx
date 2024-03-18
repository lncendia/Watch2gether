import {useState} from 'react';
import FilmRating from "../../../components/Film/FilmRating/FilmRating.tsx";
import {Film} from "../../../services/FilmsService/Models/Film.ts";
import {RatingData} from "../../../components/Rating/RatingData.ts";
import {useUser} from "../../../contexts/UserContext/UserContext.tsx";
import {useInjection} from "inversify-react";
import {IProfileService} from "../../../services/ProfileService/IProfileService.ts";

const FilmRatingModule = ({film, className}: { film: Film, className?: string }) => {

    const [rating, setRating] = useState<RatingData>(film)

    const [warning, setWarning] = useState<string>()

    const {authorizedUser} = useUser()

    const profileService = useInjection<IProfileService>('ProfileService');

    const scoreChanged = async (value: number) => {
        if (!authorizedUser) {
            setWarning("Рейтинг могут выставлять только авторизованные пользователи")
            setTimeout(() => setWarning(undefined), 2000)
            return
        }

        await profileService.addRating({filmId: film.id, score: value})

        const ratingCount = rating!.userScore ? rating!.userRatingsCount : rating!.userRatingsCount + 1
        const scoreSum = rating!.userRating * rating!.userRatingsCount - (rating!.userScore ?? 0) + value

        setRating({
            userRating: scoreSum / ratingCount,
            userRatingsCount: ratingCount,
            userScore: value
        })
    }

    return (
        <FilmRating className={className} rating={rating} isSerial={film.isSerial} userName={authorizedUser?.name}
                    warning={warning}
                    onScoreChange={scoreChanged}/>
    );
};

export default FilmRatingModule;