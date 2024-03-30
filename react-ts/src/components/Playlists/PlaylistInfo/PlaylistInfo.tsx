import ContentBlock from "../../../UI/ContentBlock/ContentBlock.tsx";
import styles from "./PlaylistInfo.module.css"
import {PlaylistInfoData} from "./PlaylistInfoData.ts";
import KeyList from "../../../UI/KeyList/KeyList.tsx";
import moment from "moment";
import FilmInfoBlock from "../../../UI/FilmInfoBlock/FilmInfoBlock.tsx";
import Divider from "../../../UI/Divider/Divider.tsx";

export interface PlaylistInfo {
    playlist: PlaylistInfoData,
    onGenreSelect: (value: string) => void,
    className?: string
}


const PlaylistInfo = (props: PlaylistInfo) => {

    return (
        <ContentBlock className={props.className}>
            <FilmInfoBlock posterClassName={styles.poster}  {...props.playlist}>
                <h3 className="m-0">{props.playlist.name}</h3>
                <Divider/>
                <div className={styles.description}>
                    {props.playlist.description}
                </div>
                <Divider/>
                <KeyList className="mb-2" title="Жанр: " values={props.playlist.genres.map(g => [g, undefined])}
                         onKeySelect={props.onGenreSelect}/>
                <KeyList className="mb-2" title="Обновлена: "
                         values={[[moment(props.playlist.updated).format("DD.MM.YYYY"), undefined]]}
                />
            </FilmInfoBlock>
        </ContentBlock>
    );
};

export default PlaylistInfo;