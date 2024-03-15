import React from 'react';
import {Offcanvas as OffcanvasBs} from "react-bootstrap";

const Offcanvas = ({title, children, show, onClose}: {
    title: string,
    show: boolean,
    onClose: () => void
    children: React.ReactNode
}) => {
    return (
        <OffcanvasBs show={show} onHide={onClose}>
            <OffcanvasBs.Header closeButton>
                <OffcanvasBs.Title>{title}</OffcanvasBs.Title>
            </OffcanvasBs.Header>
            <OffcanvasBs.Body>
                {children}
            </OffcanvasBs.Body>
        </OffcanvasBs>
    );
};

export default Offcanvas;