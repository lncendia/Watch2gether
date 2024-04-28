import React from "react";
import styles from "./FilmRoomPlayer.module.css"

interface FilmRoomPlayerProps {
    src: string,
    className?: string,
    reference: React.LegacyRef<HTMLIFrameElement>
}

const FilmRoomPlayer = ({src, className = '', reference}: FilmRoomPlayerProps) => {
    return (
        <iframe allowFullScreen className={`${styles.player} ${className}`.trim()} src={src} ref={reference}></iframe>
    );
};

export default FilmRoomPlayer;