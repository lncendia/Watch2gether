import {Nav, Navbar} from "react-bootstrap";
import styles from './Footer.module.css'
import Container from "../../../UI/Container/Container";

const Footer = () => {
    return (
        <Navbar className={styles.footer}>
            <Container>
                <span>&copy; {new Date().getFullYear()} - Overoom</span>
                <Nav>
                    <Nav.Link href="#">Правообладателям</Nav.Link>
                </Nav>
            </Container>
        </Navbar>
    );
};

export default Footer;