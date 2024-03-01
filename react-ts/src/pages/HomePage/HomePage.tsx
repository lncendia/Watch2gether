import RandomVideoWrapper from "../../modules/RandomVideoWrapper/RandomVideoWrapper.tsx";
import MyNavbar from "../../modules/Navbar/NavbarModule";
import Logo from "../../components/Logo/Logo";
import Container from "../../UI/Container/Container";
import FilmComments from "../../modules/FilmComments/components/FilmComments";
import Footer from "../../modules/Footer/Components/Footer";

const HomePage = () => {
    return (
        <div style={{background: 'black', minHeight: '100%'}}>
            <RandomVideoWrapper>
                <MyNavbar/>
                <Logo/>
            </RandomVideoWrapper>
            <Container>
                <FilmComments/>
            </Container>
            <Footer/>
        </div>
    );
};

export default HomePage;