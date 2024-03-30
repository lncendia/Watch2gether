import {Card} from "react-bootstrap";
import styles from "./FilmRoomItem.module.css";
import {FilmRoomItemData} from "./FilmRoomItemData.ts";
import FilmCard from "../../../UI/FilmCard/FilmCard.tsx";
import Svg from "../../../UI/Svg/Svg.tsx";

export interface FilmRoomItemProps {
    room: FilmRoomItemData,
    onClick: () => void
}


const FilmRoomItem = ({room, onClick}: FilmRoomItemProps) => {
    return (
        <FilmCard header={`Рейтинг: ${room.userRating}`} {...room} onClick={onClick}>
            {room.isPrivate &&
                <Svg className="me-2" width={16} height={16} fill="currentColor" viewBox="0 0 16 16">
                    <path
                        d="M8 1a2 2 0 0 1 2 2v4H6V3a2 2 0 0 1 2-2m3 6V3a3 3 0 0 0-6 0v4a2 2 0 0 0-2 2v5a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V9a2 2 0 0 0-2-2"/>
                </Svg>
            }
            <Card.Title className="d-inline">{room.title}</Card.Title>
            <Card.Text className={`mb-3 mt-2 ${styles.subtitle}`}>{room.description}</Card.Text>
            <Card.Text className={styles.viewers}>Зрителей: {room.viewersCount}</Card.Text>
            <small className={styles.type}>{room.isSerial ? "Сериал" : "Фильм"}</small>
        </FilmCard>
    );
};

export default FilmRoomItem;