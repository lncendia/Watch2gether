import ContentBlock from "../../../UI/ContentBlock/ContentBlock.tsx";
import styles from "./FilmPlayer.module.css"
import {Nav} from "react-bootstrap";
import {useState} from "react";
import {Cdn} from "../../../services/FilmsService/Models/Films.ts";

const FilmPlayer = ({cdns, className}: { cdns: Cdn[], className?: string }) => {

    const [url, setUrl] = useState(cdns[0].url)

    return (
        <ContentBlock className={className}>
            <Nav className={styles.nav} variant="tabs" defaultActiveKey={cdns[0].cdn}>
                {cdns.map(c =>
                    <Nav.Item key={c.cdn}>
                        <Nav.Link eventKey={c.cdn} onClick={() => setUrl(c.url)}>{c.cdn} ({c.quality})</Nav.Link>
                    </Nav.Item>)}
            </Nav>
            <iframe className={styles.player} src={url} allowFullScreen/>
        </ContentBlock>
    );
};

export default FilmPlayer;