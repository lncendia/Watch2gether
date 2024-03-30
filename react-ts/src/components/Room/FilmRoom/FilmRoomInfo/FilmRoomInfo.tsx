import ContentBlock from "../../../../UI/ContentBlock/ContentBlock.tsx";
import {Button} from "react-bootstrap";
import styles from "./FilmRoomInfo.module.css"
import {FilmRoomInfoData} from "./FilmRoomInfoData.ts";
import Divider from "../../../../UI/Divider/Divider.tsx";
import FilmInfoBlock from "../../../../UI/FilmInfoBlock/FilmInfoBlock.tsx";

export interface FilmRoomInfoProps {
    film: FilmRoomInfoData,
    onLeaveClicked: () => void,
    className?: string
}

const FilmRoomInfo = (props: FilmRoomInfoProps) => {
    return (
        <ContentBlock className={props.className}>
            <FilmInfoBlock posterClassName={styles.poster} {...props.film}>
                <h3 className="m-0">{props.film.title}</h3>
                <Divider/>
                <div className={styles.description}>
                    {props.film.description}
                </div>
                <Divider/>
                <Button onClick={props.onLeaveClicked} className="w-50 m-auto d-block mt-4" variant="outline-danger">
                    Покинуть комнату
                </Button>
            </FilmInfoBlock>
        </ContentBlock>
    );
};

export default FilmRoomInfo;