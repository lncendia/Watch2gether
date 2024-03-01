import {Outlet} from "react-router-dom";
import './LayoutPage.scss'
import MyNavbar from "../../modules/Navbar/NavbarModule.tsx";
import Footer from "../../modules/Footer/Components/Footer.tsx";
import Container from "../../UI/Container/Container.tsx";

// Общая страница с шаблоном для всех остальных страниц
const LayoutPage = () => {

    return (
        <>
            <MyNavbar/>
            <Container>
                <Outlet/>
            </Container>
            <Footer/>
        </>
    )
}

export default LayoutPage;