import ContentBlock from "../../../UI/ContentBlock/ContentBlock.tsx";
import {RatingData} from "../../Rating/RatingData.ts";
import Rating from "../../Rating/Rating.tsx";
import styles from "./FilmRating.module.css"

export interface FilmRatingProps {
    rating: RatingData,
    isSerial: boolean,
    userName?: string,
    onScoreChange: (value: number) => void,
    className?: string,
    warning?: string
}


const FilmRating = (props: FilmRatingProps) => {
    return (
        <ContentBlock className={props.className}>
            {props.userName && <>{props.userName.split(' ')[0]}, </>}
            {props.userName ? 'к' : 'К'}ак вам {props.isSerial ? "сериал" : "фильм"}?
            <Rating rating={props.rating} scoreChanged={props.onScoreChange}/>
            {props.warning && <div className={styles.warning}>{props.warning}</div>}
        </ContentBlock>
    );
};

export default FilmRating;