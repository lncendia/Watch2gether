import styles from "./Screamer.module.css"
import React from "react";

interface ScreamerProps {
    className?: string
    src: string
    reference: React.LegacyRef<HTMLVideoElement>
}

const Screamer = ({className = '', src, reference}: ScreamerProps) => {
    return (
        <video ref={reference} className={`${className} ${styles.video}`} src={src}/>
    );
};

export default Screamer;