import React from 'react';
import styles from "./SideText.module.css";

const SideText = ({children, className}: { children: React.ReactNode, className?: string }) => {
    return (
        <span className={`${className} ${styles.side_text}`}>
            {children}
        </span>
    );
};
export default SideText;