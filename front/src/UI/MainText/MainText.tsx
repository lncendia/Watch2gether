import React from 'react';
import styles from './MainText.module.css'

const MainText = ({children, className}: { children: React.ReactNode, className?: string }) => {
    return (
        <span className={`${className} ${styles.main_text}`}>
            {children}
        </span>
    );
};

export default MainText;