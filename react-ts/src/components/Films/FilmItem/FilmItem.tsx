import styles from "./FilmItem.module.css"
import {Card} from "react-bootstrap";
import {FilmItemData} from "./FilmItemData.ts";
import FilmCard from "../../../UI/FilmCard/FilmCard.tsx";
import GenresList from "../../../UI/GenresList/GenresList.tsx";

export interface FilmItemProps {
    film: FilmItemData,
    selectedGenre?: string,
    typeSelected: boolean,
    onClick: () => void
}

const FilmItem = ({film, selectedGenre, typeSelected, onClick}: FilmItemProps) => {

    return (
        <FilmCard {...film} onClick={onClick} header={`Рейтинг: ${film.userRating}`}>
            <Card.Title>{film.title}</Card.Title>
            <Card.Text className={`mb-3 ${styles.subtitle}`}>{film.description}</Card.Text>
            <GenresList genres={film.genres} selected={selectedGenre}/>
            <small
                className={typeSelected ? styles.type_active : styles.type}>{film.isSerial ? "Сериал" : "Фильм"}</small>
        </FilmCard>
    );
}

export default FilmItem;