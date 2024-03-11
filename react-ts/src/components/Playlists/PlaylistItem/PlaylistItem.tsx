import styles from "./PlaylistItem.module.css"
import {ReactNode} from "react";
import {Card, Col, Row} from "react-bootstrap";
import {PlaylistItemData} from "./PlaylistItemData.ts";
import moment from "moment";

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

    const genres: ReactNode[] = [];

    for (let _i = 0; _i < playlist.genres.length; _i++) {
        let className = playlist.genres[_i] === selectedGenre ? styles.genre_active : styles.genre;
        let coma = _i !== playlist.genres.length - 1;
        genres.push(
            <span className={className} key={playlist.genres[_i]}>{playlist.genres[_i]}{coma && ", "}</span>
        )
    }

    return (
        <Card className={styles.playlist} onClick={onClick}>
            <Card.Header>
                <div className="float-start">Обновлена: {moment(playlist.updated).format("DD.MM.YYYY")}</div>
            </Card.Header>
            <Row className="g-0">
                <Col lg={4}>
                    <img alt="Постер" src={playlist.posterUrl} className={styles.poster}/>
                </Col>
                <Col lg={8}>
                    <Card.Body>
                        <Card.Title>{playlist.name}</Card.Title>
                        <Card.Text className={`mb-3 ${styles.subtitle}`}>{getDescription(playlist.description)}</Card.Text>
                        <Card.Text>{genres}</Card.Text>
                    </Card.Body>
                </Col>
            </Row>
        </Card>
    );
}

export default PlaylistItem;