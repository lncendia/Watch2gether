import {Col, Row} from "react-bootstrap";
import KeyList from "../../Common/KeyList/KeyList.tsx";
import ContentBlock from "../../../UI/ContentBlock/ContentBlock.tsx";
import styles from "./FilmInfo.module.css"
import FilmWatchlist from "../FilmWatchlist/FilmWatchlist.tsx";
import {FilmInfoData} from "./FilmInfoData.ts";

export interface FilmInfoProps {
    film: FilmInfoData,
    isWatchlistEnabled: boolean,
    onCountrySelect: (value: string) => void,
    onGenreSelect: (value: string) => void,
    onPersonSelect: (value: string) => void,
    onTypeSelect: (value: string) => void,
    onYearSelect: (value: string) => void,
    inWatchlist: boolean
    onWatchlistToggle: () => void,
    className?: string
}


const FilmInfo = (props: FilmInfoProps) => {

    return (
        <ContentBlock className={props.className}>
            <Row>
                <Col xl={3} lg={4} className="text-center mb-3">
                    <div className={styles.blacker_blur}>
                        <img className={styles.poster} src={props.film.posterUrl} alt="Постер"/>
                        <div className={styles.black}></div>
                        {props.film.ratingKp &&
                            <small className={styles.kp}>KP: {props.film.ratingKp}</small>}
                        {props.film.ratingImdb &&
                            <small className={styles.imdb}>IMDB: {props.film.ratingImdb}</small>}
                    </div>
                </Col>
                <Col xl={9} lg={8}>
                    <div className="d-flex justify-content-between align-items-center">
                        <h3 className="m-0">{props.film.title}</h3>
                        <FilmWatchlist inWatchlist={props.inWatchlist} onWatchlistToggle={props.onWatchlistToggle}/>
                    </div>
                    <div className={styles.border}></div>
                    <div className={styles.description}>
                        {props.film.description}
                    </div>
                    <div className={styles.border}></div>
                    <KeyList className="mb-2" title="Год: " values={[[props.film.year.toString(), undefined]]}
                             onKeySelect={props.onYearSelect}/>
                    <KeyList className="mb-2" title="Тип: " values={[props.film.type]}
                             onKeySelect={props.onTypeSelect}/>
                    <KeyList className="mb-2" title="Страна: " values={props.film.countries}
                             onKeySelect={props.onCountrySelect}/>
                    <KeyList className="mb-2" title="Жанр: " values={props.film.genres}
                             onKeySelect={props.onGenreSelect}/>
                    <KeyList className="mb-2" title="Режиссер: " values={props.film.directors}
                             onKeySelect={props.onPersonSelect}/>
                    <KeyList className="mb-2" title="Сценарий: " values={props.film.screenWriters}
                             onKeySelect={props.onPersonSelect}/>
                    <KeyList className="mb-2" title="Актеры: "
                             values={props.film.actors}
                             onKeySelect={props.onPersonSelect}/>
                </Col>
            </Row>
        </ContentBlock>
    );
};

export default FilmInfo;