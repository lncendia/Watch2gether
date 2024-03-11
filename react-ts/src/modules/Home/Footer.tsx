import {Nav, Navbar} from "react-bootstrap";
import styles from './Footer.module.css'
import Container from "../../UI/Container/Container.tsx";

const Footer = () => {
    return (
        <Navbar className={styles.footer}>
            <Container>
                <Nav>
                    <Nav.Item>&copy; {new Date().getFullYear()} - Overoom</Nav.Item>
                </Nav>
                <Nav>
                    <Nav.Link className="p-0">Правообладателям</Nav.Link>
                </Nav>
            </Container>
        </Navbar>
    );
};

export default Footer;