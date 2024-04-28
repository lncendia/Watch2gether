import {Modal} from "react-bootstrap";
import {ReactNode} from "react";

interface ChangeNameProps {
    show: boolean
    onHide: () => void,
    children: ReactNode
}

const ChangeName = ({show, onHide, children}: ChangeNameProps) => {

    return (
        <Modal show={show} onHide={onHide}>
            <Modal.Header closeButton>
                <Modal.Title>Изменить имя</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                {children}
            </Modal.Body>
        </Modal>
    );
};

export default ChangeName;