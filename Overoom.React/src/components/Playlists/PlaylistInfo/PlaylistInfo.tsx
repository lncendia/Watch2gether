import ContentBlock from "../../../UI/ContentBlock/ContentBlock.tsx";
import styles from "./PlaylistInfo.module.css"
import KeyList from "../../../UI/KeyList/KeyList.tsx";
import moment from "moment";
import FilmInfoCard from "../../../UI/FilmInfoCard/FilmInfoCard.tsx";
import Divider from "../../../UI/Divider/Divider.tsx";

export interface PlaylistInfo {
    onGenreSelect: (value: string) => void,
    className?: string
    name: string
    description: string
    posterUrl: string
    updated: Date,
    genres: string[]
}


const PlaylistInfo = (props: PlaylistInfo) => {

    return (
        <ContentBlock className={props.className}>
            <FilmInfoCard posterClassName={styles.poster}  {...props}>
                <h3 className="m-0">{props.name}</h3>
                <Divider/>
                <div className={styles.description}>
                    {props.description}
                </div>
                <Divider/>
                <KeyList className="mb-2" title="Жанр: " values={props.genres.map(g => [g, undefined])}
                         onKeySelect={props.onGenreSelect}/>
                <KeyList className="mb-2" title="Обновлена: "
                         values={[[moment(props.updated).format("DD.MM.YYYY"), undefined]]}
                />
            </FilmInfoCard>
        </ContentBlock>
    );
};

export default PlaylistInfo;