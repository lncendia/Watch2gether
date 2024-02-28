import React from 'react';
import styles from "./Background.module.css"

const Background = ({children}: { children: React.ReactNode }) => {
    return (
        <div className={styles.background}>
            {children}
        </div>
    );
};

export default Background;