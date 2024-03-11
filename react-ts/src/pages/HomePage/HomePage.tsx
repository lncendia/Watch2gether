import RandomVideoWrapper from "../../modules/Home/RandomVideoWrapper.tsx";
import NavbarModule from "../../modules/Home/NavbarModule.tsx";
import Logo from "../../components/Logo/Logo";
import Container from "../../UI/Container/Container";
import FilmCommentsModule from "../../modules/Film/FilmCommentsModule/FilmCommentsModule.tsx";
import Footer from "../../modules/Home/Footer.tsx";

const HomePage = () => {
    return (
        <div style={{background: 'black', minHeight: '100%'}}>
            <RandomVideoWrapper>
                <NavbarModule/>
                <Logo/>
            </RandomVideoWrapper>
            <Container>
                <FilmCommentsModule/>
            </Container>
            <Footer/>
        </div>
    );
};

export default HomePage;