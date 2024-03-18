import {ViewerData} from "../Viewer/ViewerData.ts";
import Viewer from "../Viewer/Viewer.tsx";
import styles from "./ViewersList.module.css"
import {useState} from "react";
import {Button} from "react-bootstrap";


interface ViewersListProps {
    viewers: ViewerData[]
    currentId: string,
    ownerId: string,
}

const ViewersList = (props: ViewersListProps) => {
    const [show, setShow] = useState(false)

    const currentViewer = props.viewers.filter(v => v.id === props.currentId)[0]
    const isCurrentOwner = props.currentId === props.ownerId

    return (
        <div className={styles.viewers}>
            <div className="d-flex justify-content-between align-items-center">

                <Viewer viewer={currentViewer} owner={isCurrentOwner} showBeep={false}
                        showChange={currentViewer.change} showKick={false} showScream={false}
                        showSync={!isCurrentOwner}/>

                <Button variant="outline-primary" active={show} onClick={() => setShow(v => !v)}>
                    <svg xmlns="http://www.w3.org/2000/svg" className={styles.menu} viewBox="0 0 16 16">
                        <path fillRule="evenodd"
                              d="M2.5 12a.5.5 0 0 1 .5-.5h10a.5.5 0 0 1 0 1H3a.5.5 0 0 1-.5-.5m0-4a.5.5 0 0 1 .5-.5h10a.5.5 0 0 1 0 1H3a.5.5 0 0 1-.5-.5m0-4a.5.5 0 0 1 .5-.5h10a.5.5 0 0 1 0 1H3a.5.5 0 0 1-.5-.5"/>
                    </svg>
                </Button>

            </div>
            {show && props.viewers.filter(v => v.id != props.currentId).map(v =>
                (
                    <Viewer className="mt-3" key={v.id} viewer={v} owner={v.id === props.ownerId} showBeep={v.beep}
                            showScream={v.scream} showChange={v.change} showKick={isCurrentOwner} showSync={false}/>
                )
            )}
        </div>
    );
};

export default ViewersList;