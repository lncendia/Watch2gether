import RandomVideoWrapper from "../../modules/RandomVideoWrapper/components/RandomVideoWrapper";
import MyNavbar from "../../modules/Navbar/NavbarModule";
import Logo from "../../components/Logo/Logo";
import Container from "../../UI/Container/Container";
import FilmComments from "../../modules/FilmComments/components/FilmComments";
import Footer from "../../modules/Footer/Components/Footer";

const HomePage = () => {
    return (
        <div style={{background: 'black', minHeight:'100%'}}>
            <RandomVideoWrapper>
                <MyNavbar color="transparent" linkColor="#d2d2d2" linkActiveColor="#8a8a8a"/>
                <Logo/>
            </RandomVideoWrapper>
            <Container>
                <FilmComments/>
            </Container>
            <Footer color="dark"/>
        </div>
    );
};

export default HomePage;