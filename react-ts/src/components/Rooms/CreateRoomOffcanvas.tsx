import React from 'react';
import {Offcanvas} from "react-bootstrap";

const CreateRoomOffcanvas = ({title, children, show, onClose}: {
    title: string,
    show: boolean,
    onClose: () => void
    children: React.Node
}) => {
    return (
        <Offcanvas show={show} onHide={onClose}>
            <Offcanvas.Header closeButton>
                <Offcanvas.Title>{title}</Offcanvas.Title>
            </Offcanvas.Header>
            <Offcanvas.Body>
                {children}
            </Offcanvas.Body>
        </Offcanvas>
    );
};

export default CreateRoomOffcanvas;