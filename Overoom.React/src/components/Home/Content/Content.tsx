import {Col, Row} from "react-bootstrap";
import styles from "./Content.module.css"

const Content = ({src, text, reverse = false}: { src: string, text: string, reverse?: boolean }) => {
    return (
        <Row className={`${styles.block} p-md-2 p-lg-4`}>
            <Col xl={7} className={reverse ? "order-xl-last" : ""}>
                <img className={styles.img} src={src} alt=""/>
            </Col>
            <Col xl={3}>
                <span className={styles.text}>{text}</span>
            </Col>
        </Row>
    );
};

export default Content;