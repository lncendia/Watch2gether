import {Outlet} from "react-router-dom";
import NavbarModule from "../../modules/Home/NavbarModule.tsx";
import Footer from "../../modules/Home/Footer.tsx";
import Container from "../../UI/Container/Container.tsx";

// Общая страница с шаблоном для всех остальных страниц
const LayoutPage = () => {

    return (
        <>
            <NavbarModule/>
            <Container>
                <Outlet/>
            </Container>
            <Footer/>
        </>
    )
}

export default LayoutPage;