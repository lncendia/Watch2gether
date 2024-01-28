import React from 'react';
import styles from './CardLink.module.css'

const CardLink = ({className = '', href, children}: {
    className?: string,
    href: string,
    children: React.ReactNode
}) => {

    return (
        <a className={`${className} ${styles.link}`.trim()} href={href}>
            {children}
        </a>
    );
};

export default CardLink;