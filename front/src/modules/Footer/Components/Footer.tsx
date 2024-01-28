import React from 'react';
import {NavLink} from "react-bootstrap";
import styles from './Footer.module.css'
import Container from "../../../UI/Container/Container";

const Footer = ({color}: { color: string }) => {
    return (
        <footer className={`${'bg-' + color} ${styles.footer}`}>
            <Container>
                <div className="d-flex justify-content-between">
                    <span className={styles.navLinkf}>&copy; {new Date().getFullYear()} - Overoom</span>
                    <NavLink className={styles.nav_link} href="#">Правообладателям</NavLink>
                </div>
            </Container>
        </footer>
    );
};

export default Footer;