import RandomVideoModule from "../../modules/Home/RandomVideoModule/RandomVideoModule.tsx";
import NavbarModule from "../../modules/Home/NavbarModule/NavbarModule.tsx";
import Logo from "../../components/Home/Logo/Logo";
import Footer from "../../modules/Home/Footer.tsx";
import Content from "../../components/Home/Content/Content.tsx";
import Container from "../../UI/Container/Container.tsx";
import BlockTitle from "../../UI/BlockTitle/BlockTitle.tsx";

const catalogInfo = "На главной странице сайта вы можете найти список фильмов, которые доступны для просмотра. Выберите фильм, нажав на его обложку, и начните просмотр."
const filmRoomsInfo = "Если вы хотите создать комнату для совместного просмотра фильма, нажмите кнопку \"Создать комнату\". Вы можете пригласить друзей, отправив им ссылку на комнату или сделать её открытой, дав возможность другим пользователям сайта подключится к вам."
const roomInfo = "Во время просмотра фильма или видео в комнате вы можете обмениваться сообщениями с другими участниками. Для этого введите текст в поле ввода и нажмите \"Отправить\"."
const profileInfo = "В своем профиле вы можете посмотреть историю просмотров и оценок, а также список фильмов, которые вы добавили в \"Смотреть позже\"."
const allowsInfo = "Во время просмотра фильма или видео в комнате вы можете издать звуковой сигнал или активировать скример, нажав соответствующие кнопки. Эти функции можно отключить в настройках."


const HomePage = () => {
    return (
        <>
            <RandomVideoModule>
                <NavbarModule/>
                <Logo/>
            </RandomVideoModule>
            <Container>
                <BlockTitle className={"mt-5"} title="Каталог"/>
                <Content src="img/catalog.png" text={catalogInfo}/>
                <BlockTitle className={"mt-5"} title="Комнаты"/>
                <Content src="img/filmRooms.png" reverse={true} text={filmRoomsInfo}/>
                {/*<Content src={} text={roomInfo}/>*/}
                <BlockTitle className={"mt-5"} title="Профиль"/>
                <Content src="img/profile.png" text={profileInfo}/>
                {/*<Content src={} text={allowsInfo}/>*/}
            </Container>
            <Footer/>
        </>
    );
};

export default HomePage;