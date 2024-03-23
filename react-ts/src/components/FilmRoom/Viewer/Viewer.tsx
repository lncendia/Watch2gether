import {ViewerData} from "./ViewerData.ts";
import moment from "moment";
import styles from "./Viewer.module.css"
import RoomAvatar from "../../../UI/RoomAvatar/RoomAvatar.tsx";
import {NavDropdown} from "react-bootstrap";

const pauseComponent = (pause: boolean) => {
    if (pause) {
        return (
            <svg className={styles.icon} xmlns="http://www.w3.org/2000/svg" viewBox="0 0 16 16">
                <path
                    d="M5.5 3.5A1.5 1.5 0 0 1 7 5v6a1.5 1.5 0 0 1-3 0V5a1.5 1.5 0 0 1 1.5-1.5m5 0A1.5 1.5 0 0 1 12 5v6a1.5 1.5 0 0 1-3 0V5a1.5 1.5 0 0 1 1.5-1.5"/>
            </svg>
        )
    }

    return (
        <svg className={styles.icon} xmlns="http://www.w3.org/2000/svg" viewBox="0 0 16 16">
            <path
                d="m11.596 8.697-6.363 3.692c-.54.313-1.233-.066-1.233-.697V4.308c0-.63.692-1.01 1.233-.696l6.363 3.692a.802.802 0 0 1 0 1.393"/>
        </svg>
    )
}

const fullscreenComponent = (fullscreen: boolean) => {
    if (fullscreen) {
        return (
            <svg className={styles.icon} xmlns="http://www.w3.org/2000/svg" viewBox="0 0 16 16">
                <path
                    d="M1.5 1a.5.5 0 0 0-.5.5v4a.5.5 0 0 1-1 0v-4A1.5 1.5 0 0 1 1.5 0h4a.5.5 0 0 1 0 1zM10 .5a.5.5 0 0 1 .5-.5h4A1.5 1.5 0 0 1 16 1.5v4a.5.5 0 0 1-1 0v-4a.5.5 0 0 0-.5-.5h-4a.5.5 0 0 1-.5-.5M.5 10a.5.5 0 0 1 .5.5v4a.5.5 0 0 0 .5.5h4a.5.5 0 0 1 0 1h-4A1.5 1.5 0 0 1 0 14.5v-4a.5.5 0 0 1 .5-.5m15 0a.5.5 0 0 1 .5.5v4a1.5 1.5 0 0 1-1.5 1.5h-4a.5.5 0 0 1 0-1h4a.5.5 0 0 0 .5-.5v-4a.5.5 0 0 1 .5-.5"/>
            </svg>
        )
    }

    return (
        <svg className={styles.icon} xmlns="http://www.w3.org/2000/svg" viewBox="0 0 16 16">
            <path
                d="M5.5 0a.5.5 0 0 1 .5.5v4A1.5 1.5 0 0 1 4.5 6h-4a.5.5 0 0 1 0-1h4a.5.5 0 0 0 .5-.5v-4a.5.5 0 0 1 .5-.5m5 0a.5.5 0 0 1 .5.5v4a.5.5 0 0 0 .5.5h4a.5.5 0 0 1 0 1h-4A1.5 1.5 0 0 1 10 4.5v-4a.5.5 0 0 1 .5-.5M0 10.5a.5.5 0 0 1 .5-.5h4A1.5 1.5 0 0 1 6 11.5v4a.5.5 0 0 1-1 0v-4a.5.5 0 0 0-.5-.5h-4a.5.5 0 0 1-.5-.5m10 1a1.5 1.5 0 0 1 1.5-1.5h4a.5.5 0 0 1 0 1h-4a.5.5 0 0 0-.5.5v4a.5.5 0 0 1-1 0z"/>
        </svg>
    )
}

interface ViewerProps {
    viewer: ViewerData
    className?: string,
    showBeep: boolean,
    showScream: boolean,
    showChange: boolean,
    showKick: boolean,
    showSync: boolean
    owner: boolean,
    onBeep: () => void
    onScream: () => void
}

const Viewer = (props: ViewerProps) => {
    return (
        <div className={`d-flex justify-content-start align-items-center ${props.className ?? ''}`}>
            <RoomAvatar owner={props.owner} src={props.viewer.photoUrl}/>
            <div className="ms-3">
                <div className="d-flex justify-content-start align-items-center">
                    <NavDropdown className="me-2" title={props.viewer.username}>
                        {props.showSync && <NavDropdown.Item>Синхронизовать</NavDropdown.Item>}
                        {props.showKick && <NavDropdown.Item>Выгнать</NavDropdown.Item>}
                        {props.showBeep && <NavDropdown.Item onClick={props.onBeep}>Разбудить</NavDropdown.Item>}
                        {props.showScream && <NavDropdown.Item onClick={props.onScream}>Напугать</NavDropdown.Item>}
                        {props.showChange && <NavDropdown.Item>Изменить имя</NavDropdown.Item>}
                    </NavDropdown>
                    {pauseComponent(props.viewer.pause)}
                    {fullscreenComponent(props.viewer.fullScreen)}
                </div>
                <div className={styles.timer}>
                    {moment.utc(props.viewer.second * 1000).format('HH:mm:ss')}
                    {props.viewer.current && <span> ({props.viewer.current})</span>}
                </div>
            </div>
        </div>
    );
};

export default Viewer;