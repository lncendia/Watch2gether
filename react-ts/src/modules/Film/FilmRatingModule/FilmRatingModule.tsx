import {useCallback, useState} from 'react';
import FilmRating from "../../../components/Film/FilmRating/FilmRating.tsx";
import {RatingData} from "../../../components/Rating/RatingData.ts";
import {useUser} from "../../../contexts/UserContext/UserContext.tsx";
import {useInjection} from "inversify-react";
import {IProfileService} from "../../../services/ProfileService/IProfileService.ts";

interface FilmRatingModuleProps {
    id: string,
    userRating: number,
    userRatingsCount: number,
    userScore?: number,
    isSerial: boolean,
    className?: string
}

const FilmRatingModule = (props: FilmRatingModuleProps) => {

    const [rating, setRating] = useState<RatingData>({...props})

    const [warning, setWarning] = useState<string>()

    const {authorizedUser} = useUser()

    const profileService = useInjection<IProfileService>('ProfileService');

    const scoreChanged = useCallback(async (value: number) => {

        if (!authorizedUser) {
            setWarning("Рейтинг могут выставлять только авторизованные пользователи")
            setTimeout(() => setWarning(undefined), 2000)
            return
        }

        await profileService.addRating({filmId: props.id, score: value})

        const ratingCount = rating!.userScore ? rating!.userRatingsCount : rating!.userRatingsCount + 1
        const scoreSum = rating!.userRating * rating!.userRatingsCount - (rating!.userScore ?? 0) + value

        setRating({
            userRating: scoreSum / ratingCount,
            userRatingsCount: ratingCount,
            userScore: value
        })
    }, [authorizedUser, profileService, props.id, rating])

    return (
        <FilmRating className={props.className} rating={rating} isSerial={props.isSerial}
                    userName={authorizedUser?.name}
                    warning={warning}
                    onScoreChange={scoreChanged}/>
    );
};

export default FilmRatingModule;