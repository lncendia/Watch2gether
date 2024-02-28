import React from 'react';
import styles from './VideoWrapper.module.css'

const VideoWrapper = ({children, src}: { children: React.ReactNode, src: string }) => {
    return (
        <div className={styles.video_wrapper}>
            <video className={styles.video} playsInline autoPlay muted loop>
                <source src={src} type="video/mp4"/>
                Your browser does not support the video tag.
            </video>
            <div className={styles.relative}>
                {children}
            </div>
        </div>
    );
};

export default VideoWrapper;