import {Card} from "react-bootstrap";
import {PlaylistItemData} from "./PlaylistItemData.ts";
import moment from "moment";
import FilmCard from "../../../UI/FilmCard/FilmCard.tsx";
import GenresList from "../../../UI/GenresList/GenresList.tsx";
import styles from "./PlaylistItem.module.css"

export interface PlaylistItemProps {
    playlist: PlaylistItemData,
    selectedGenre?: string,
    onClick: () => void
}

const getDescription = (desc: string) => {
    if (desc.length > 150)
        return desc.slice(0, 150) + '...'
    return desc
}

const PlaylistItem = ({playlist, selectedGenre, onClick}: PlaylistItemProps) => {

    return (
        <FilmCard header={`Обновлена: ${moment(playlist.updated).format("DD.MM.YYYY")}`} {...playlist} onClick={onClick}>
            <Card.Title>{playlist.name}</Card.Title>
            <Card.Text
                className={`mb-3 ${styles.subtitle}`}>{getDescription(playlist.description)}</Card.Text>
            <Card.Text>
                <GenresList genres={playlist.genres} selected={selectedGenre}/>
            </Card.Text>
        </FilmCard>
    );
}

export default PlaylistItem;