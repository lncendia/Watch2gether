import {ViewerData} from "./ViewerData.ts";
import moment from "moment";
import styles from "./Viewer.module.css"
import RoomAvatar from "../../../../UI/RoomAvatar/RoomAvatar.tsx";
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
        <svg className={styles.icon} xmlns="http://www.w3.org/2000/svg" viewBox="-2 -2 28 28">
            <path
                d="M7 9.5C8.38071 9.5 9.5 8.38071 9.5 7V2.5C9.5 1.94772 9.05228 1.5 8.5 1.5H7.5C6.94772 1.5 6.5 1.94772 6.5 2.5V6.5H2.5C1.94772 6.5 1.5 6.94772 1.5 7.5V8.5C1.5 9.05228 1.94772 9.5 2.5 9.5H7Z"/>
            <path
                d="M17 9.5C15.6193 9.5 14.5 8.38071 14.5 7V2.5C14.5 1.94772 14.9477 1.5 15.5 1.5H16.5C17.0523 1.5 17.5 1.94772 17.5 2.5V6.5H21.5C22.0523 6.5 22.5 6.94772 22.5 7.5V8.5C22.5 9.05228 22.0523 9.5 21.5 9.5H17Z"/>
            <path
                d="M17 14.5C15.6193 14.5 14.5 15.6193 14.5 17V21.5C14.5 22.0523 14.9477 22.5 15.5 22.5H16.5C17.0523 22.5 17.5 22.0523 17.5 21.5V17.5H21.5C22.0523 17.5 22.5 17.0523 22.5 16.5V15.5C22.5 14.9477 22.0523 14.5 21.5 14.5H17Z"/>
            <path
                d="M9.5 17C9.5 15.6193 8.38071 14.5 7 14.5H2.5C1.94772 14.5 1.5 14.9477 1.5 15.5V16.5C1.5 17.0523 1.94772 17.5 2.5 17.5H6.5V21.5C6.5 22.0523 6.94772 22.5 7.5 22.5H8.5C9.05228 22.5 9.5 22.0523 9.5 21.5V17Z"/>
        </svg>
    )
}

const typeComponent = (
    <svg className={`${styles.type} ${styles.icon}`} xmlns="http://www.w3.org/2000/svg" viewBox="0 0 16 16">
        <path
            d="M16 8c0 3.866-3.582 7-8 7a9.06 9.06 0 0 1-2.347-.306c-.584.296-1.925.864-4.181 1.234-.2.032-.352-.176-.273-.362.354-.836.674-1.95.77-2.966C.744 11.37 0 9.76 0 8c0-3.866 3.582-7 8-7s8 3.134 8 7zM5 8a1 1 0 1 0-2 0 1 1 0 0 0 2 0zm4 0a1 1 0 1 0-2 0 1 1 0 0 0 2 0zm3 1a1 1 0 1 0 0-2 1 1 0 0 0 0 2z"/>
    </svg>
)

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
    onChange: () => void
}

const Viewer = (props: ViewerProps) => {
    return (
        <div className={`d-flex justify-content-start align-items-center ${props.className ?? ''}`}>
            <RoomAvatar owner={props.owner} src={props.viewer.photoUrl}/>
            <div className="ms-3">
                {props.viewer.online && <OnlineViewer {...props}/>}
                {!props.viewer.online && <OfflineViewer {...props}/>}
            </div>
        </div>
    );
};


const OnlineViewer = (props: ViewerProps) => {

    return (
        <>
            <div className="d-flex justify-content-start align-items-center">
                <Username {...props}/>
                {pauseComponent(props.viewer.pause)}
                {fullscreenComponent(props.viewer.fullScreen)}
                {props.viewer.typing && typeComponent}
            </div>
            <div className={styles.timer}>
                {moment.utc(props.viewer.second * 1000).format('HH:mm:ss')}
                {props.viewer.current && <span> ({props.viewer.current})</span>}
            </div>
        </>
    )
}


const OfflineViewer = (props: ViewerProps) => {

    return (
        <>
            <Username {...props}/>
            <div className={`${styles.offline} ${styles.offlineBadge}`}>offline</div>
        </>
    )
}


const Username = (props: ViewerProps) => {
    const anyActions = props.showBeep || props.showScream || props.showChange || props.showKick || props.showSync

    const className = props.viewer.online ? '' : styles.offline

    if (anyActions) {
        return (
            <NavDropdown className={`me-2 ${className}`.trim()} title={props.viewer.username}>
                {props.showSync && props.viewer.online && <NavDropdown.Item>Синхронизовать</NavDropdown.Item>}
                {props.showKick && <NavDropdown.Item>Выгнать</NavDropdown.Item>}
                {props.showBeep && props.viewer.online &&
                    <NavDropdown.Item onClick={props.onBeep}>Разбудить</NavDropdown.Item>}
                {props.showScream && props.viewer.online &&
                    <NavDropdown.Item onClick={props.onScream}>Напугать</NavDropdown.Item>}
                {props.showChange && props.viewer.online &&
                    <NavDropdown.Item onClick={props.onChange}>Изменить имя</NavDropdown.Item>}
            </NavDropdown>
        )
    }
    return <div className={className}>{props.viewer.username}</div>
}

export default Viewer;