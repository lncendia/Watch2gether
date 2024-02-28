import {Outlet} from "react-router-dom";
import './LayoutPage.scss'
import MyNavbar from "../../modules/Navbar/NavbarModule.tsx";
import Footer from "../../modules/Footer/Components/Footer.tsx";
import Background from "../../UI/Background/Background.tsx";
import Container from "../../UI/Container/Container.tsx";

// Общая страница с шаблоном для всех остальных страниц
const LayoutPage = () => {

    return (
        <Background data-bs-theme="light">
            <MyNavbar color="dark" linkColor="#d2d2d2" linkActiveColor="#8a8a8a"/>
            <Container>
                <Outlet/>
            </Container>
            <Footer color="dark"/>
        </Background>
    )
}

export default LayoutPage;