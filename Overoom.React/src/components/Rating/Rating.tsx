import styles from "./Rating.module.css"
import {useState} from "react";
import {RatingData} from "./RatingData.ts";


interface RatingProps {
    rating: RatingData
    scoreChanged: (score: number) => void,
    className?: string
}

const getReviewLabel = (rating: number) => {
    switch (rating) {
        case 1:
            return 'Ужасно 🤮';
        case 2:
            return 'Плохо 🥺';
        case 3:
            return 'Удовлетворительно ☹️';
        case 4:
            return 'Хорошо 😌';
        case 5:
            return 'Очень хорошо 😃';
        case 6:
            return 'Отлично 😇';
        case 7:
            return 'Замечательно 👏';
        case 8:
            return 'Супер 😱';
        case 9:
            return 'Великолепно 🤩';
        case 10:
            return 'Превосходно 🤯';
        default:
            return '';
    }
};

const getScoresCountString = (count: number) => {
    count = count % 10
    if (count === 1) return 'оценка'
    if (count > 1 && count < 5) return 'оценки'
    return 'оценок'
}

const Rating = ({className, rating, scoreChanged}: RatingProps) => {

    const [hoverRating, setHoverRating] = useState<number>()


    return (
        <div className={className ?? ''}>
            <div className="d-flex align-items-center">
                {[...Array(10)].map((_, index) => {
                    index += 1;
                    let viewedScore: number
                    let onStyle: string
                    if (hoverRating) {
                        viewedScore = hoverRating;
                        onStyle = styles.user_on;
                    } else if (rating.userScore) {
                        viewedScore = rating.userScore
                        onStyle = styles.user_on;
                    } else {
                        viewedScore = rating.userRating
                        onStyle = styles.on;
                    }
                    return (
                        <button
                            type="button"
                            key={index}
                            className={`${styles.score} ${(index <= viewedScore ? onStyle : styles.off)}`}
                            onClick={() => scoreChanged(index)}
                            onMouseEnter={() => setHoverRating(index)}
                            onMouseLeave={() => setHoverRating(undefined)}
                        >
                            <span className={styles.star}>&#9733;</span>
                        </button>
                    );
                })}
                <div className={styles.label}>
                    {getReviewLabel(hoverRating ?? 0)}
                </div>
            </div>
            <div
                className={styles.info}>{rating.userRating}&#9733;, {rating.userRatingsCount} {getScoresCountString(rating.userRatingsCount)}</div>
        </div>
    );
};

export default Rating;