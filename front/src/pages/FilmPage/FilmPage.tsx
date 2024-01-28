import React, {useState} from 'react';
import MyNavbar from "../../modules/Navbar/Components/Navbar";
import Container from "../../UI/Container/Container";
import Footer from "../../modules/Footer/Components/Footer";
import FilmsCatalog from "../../modules/FilmsCatalog/components/FilmsCatalog";
import Background from "../../UI/Background/Background";
import GenreSelect from "../../modules/GenreSelect/GenreSelect";

const FilmPage = () => {

    const [genre, genreSelect] = useState('')

    return (
        <Background>
            <MyNavbar color="dark" linkColor="#d2d2d2" linkActiveColor="#8a8a8a"/>
            <Container>
                <GenreSelect className="pb-3" genreSelected={g => genreSelect(g)}/>
                <FilmsCatalog genre={genre}/>
            </Container>
            <Footer color="dark"/>
        </Background>
    );
};

export default FilmPage;