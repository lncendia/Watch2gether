import React from "react";

interface FilmRoomPlayerProps {
    src: string,
    className?: string,
    reference: React.LegacyRef<HTMLIFrameElement>
}

const FilmRoomPlayer = ({src, className, reference}: FilmRoomPlayerProps) => {
    return (
        <iframe allowFullScreen width={640} height={300} src={src} className={className ?? ""} ref={reference}></iframe>
    );
};

export default FilmRoomPlayer;