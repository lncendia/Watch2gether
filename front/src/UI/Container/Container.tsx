import React from 'react';
import styles from './Container.module.css'
import {Container as ContainerBs} from "react-bootstrap";

const Container = ({children, className}: { children: React.ReactNode, className?: string }) => {
    return (
        <ContainerBs className={`${className} ${styles.container}`}>
            {children}
        </ContainerBs>
    );
};

export default Container;