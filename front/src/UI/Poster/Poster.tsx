import React from 'react';
import {Card} from "react-bootstrap";
import styles from "./Poster.module.css"

const Poster = ({src, className = '', alt = 'Постер'}: { src: string, className?: string, alt?: string }) => {
    return (
        <Card.Img alt={alt} src={src} className={`${className} ${styles.poster}`.trim()} variant="top"/>
    );
};

export default Poster;