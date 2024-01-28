import React from 'react';
import styles from "./Background.module.css"

const Background = ({children, className = ''}: { children: React.ReactNode, className?: string }) => {
    return (
        <div className={`${className} ${styles.background}`.trim()}>
            {children}
        </div>
    );
};

export default Background;