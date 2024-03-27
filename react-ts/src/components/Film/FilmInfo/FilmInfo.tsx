import {Button, Col, Row} from "react-bootstrap";
import KeyList from "../../Common/KeyList/KeyList.tsx";
import ContentBlock from "../../../UI/ContentBlock/ContentBlock.tsx";
import styles from "./FilmInfo.module.css"
import FilmWatchlist from "../FilmWatchlist/FilmWatchlist.tsx";
import {FilmInfoData} from "./FilmInfoData.ts";
import FilmPoster from "../../../UI/FilmPoster/FilmPoster.tsx";
import Divider from "../../../UI/Divider/Divider.tsx";

export interface FilmInfoProps {
    film: FilmInfoData,
    isWatchlistEnabled: boolean,
    onCountrySelect: (value: string) => void,
    onGenreSelect: (value: string) => void,
    onPersonSelect: (value: string) => void,
    onTypeSelect: (value: string) => void,
    onYearSelect: (value: string) => void,
    onRoomCreateClicked: () => void,
    inWatchlist: boolean,
    onWatchlistToggle: () => void,
    className?: string
}


const FilmInfo = (props: FilmInfoProps) => {

    return (
        <ContentBlock className={props.className}>
            <Row>
                <Col xl={3} lg={4} className="text-center mb-3">
                    <FilmPoster className={styles.poster} posterUrl={props.film.posterUrl}
                                ratingKp={props.film.ratingKp} ratingImdb={props.film.ratingImdb}/>
                </Col>
                <Col xl={9} lg={8}>
                    <div className="d-flex justify-content-between align-items-center">
                        <h3 className="m-0">{props.film.title}</h3>
                        <FilmWatchlist inWatchlist={props.inWatchlist} onWatchlistToggle={props.onWatchlistToggle}/>
                    </div>
                    <Divider/>
                    <div className={styles.description}>
                        {props.film.description}
                    </div>
                    <Divider/>
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

                    <Button onClick={props.onRoomCreateClicked} className="w-50 mt-4" variant="outline-danger">
                        Создать комнату
                    </Button>
                </Col>
            </Row>
        </ContentBlock>
    );
};

export default FilmInfo;