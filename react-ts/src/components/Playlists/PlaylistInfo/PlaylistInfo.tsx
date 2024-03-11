import {Col, Row} from "react-bootstrap";
import ContentBlock from "../../../UI/ContentBlock/ContentBlock.tsx";
import styles from "./PlaylistInfo.module.css"
import {PlaylistInfoData} from "./PlaylistInfoData.ts";
import KeyList from "../../Common/KeyList/KeyList.tsx";
import moment from "moment";

export interface PlaylistInfo {
    playlist: PlaylistInfoData,
    onGenreSelect: (value: string) => void,
    className?: string
}


const PlaylistInfo = (props: PlaylistInfo) => {

    return (
        <ContentBlock className={props.className}>
            <Row>
                <Col xl={3} lg={4} className="text-center mb-3">
                    <img className={styles.poster} src={props.playlist.posterUrl} alt="Постер"/>
                </Col>
                <Col xl={9} lg={8}>
                    <h3 className="m-0">{props.playlist.name}</h3>
                    <div className={styles.border}></div>
                    <div className={styles.description}>
                        {props.playlist.description}
                    </div>
                    <div className={styles.border}></div>
                    <KeyList className="mb-2" title="Жанр: " values={props.playlist.genres.map(g => [g, undefined])}
                             onKeySelect={props.onGenreSelect}/>
                    <KeyList className="mb-2" title="Обновлена: "
                             values={[[moment(props.playlist.updated).format("DD.MM.YYYY"), undefined]]}
                    />
                </Col>
            </Row>
        </ContentBlock>
    );
};

export default PlaylistInfo;