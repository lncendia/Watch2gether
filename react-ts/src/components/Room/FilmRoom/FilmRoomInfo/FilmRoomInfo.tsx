import ContentBlock from "../../../../UI/ContentBlock/ContentBlock.tsx";
import {Button, Col, Row} from "react-bootstrap";
import styles from "./FilmRoomInfo.module.css"
import {FilmRoomInfoData} from "./FilmRoomInfoData.ts";
import FilmPoster from "../../../../UI/FilmPoster/FilmPoster.tsx";
import Divider from "../../../../UI/Divider/Divider.tsx";

export interface FilmRoomInfoProps {
    film: FilmRoomInfoData,
    onLeaveClicked: () => void,
    className?: string
}

const FilmRoomInfo = (props: FilmRoomInfoProps) => {
    return (
        <ContentBlock className={props.className}>
            <Row>
                <Col xl={3} lg={4} className="text-center mb-3">
                    <FilmPoster className={styles.poster} posterUrl={props.film.posterUrl}
                                ratingKp={props.film.ratingKp} ratingImdb={props.film.ratingImdb}/>
                </Col>
                <Col xl={9} lg={8}>
                    <Divider/>
                    <div className={styles.description}>
                        {props.film.description}
                    </div>
                    <Divider/>
                    <Button onClick={props.onLeaveClicked} className="w-50 m-auto d-block mt-4" variant="outline-danger">
                        Покинуть комнату
                    </Button>
                </Col>
            </Row>
        </ContentBlock>
    );
};

export default FilmRoomInfo;