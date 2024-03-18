import Container from "../../../UI/Container/Container.tsx";
import NavLogo from "../NavLogo/NavLogo.tsx";
import {Nav, Navbar as NavbarBs, NavDropdown} from "react-bootstrap";
import Svg from "../../../UI/Svg/Svg.tsx";
import {AuthorizedUser} from "../../../contexts/UserContext/AuthorizedUser.ts";
import FilmSearch from "../FilmSearch/FilmSearch.tsx";
import styles from "./Navbar.module.css"
import {FilmShort} from "../../../services/FilmsService/Models/Films.ts";

export interface MenuProps {
    films: FilmShort[],
    onFilmSearch: (value: string) => void,
    user: AuthorizedUser | null,
    onLogout: () => void,
    onLogin: () => void,
    onCatalog: () => void,
    onPlaylists: () => void,
    onRooms: () => void,
    onYouTube: () => void,
    onProfile: () => void,
    onHome: () => void,
    onFilm: (film: FilmShort) => void
}

const Navbar = (props: MenuProps) => {
    return (
        <NavbarBs expand="xxl">
            <Container>
                <NavLogo onClick={props.onHome}/>
                <NavbarBs.Toggle aria-controls="navbarSupportedContent" aria-expanded="false"
                               aria-label="Переключатель навигации">
                    <span className="navbar-toggler-icon"></span>
                </NavbarBs.Toggle>
                <NavbarBs.Collapse id="navbarSupportedContent">
                    <Nav className="flex-grow-1">
                        <Nav.Link onClick={props.onCatalog}>
                            <Svg width={16} height={16} fill="currentColor"
                                 className="bi bi-camera-reels-fill" viewBox="0 0 16 16">
                                <path d="M6 3a3 3 0 1 1-6 0 3 3 0 0 1 6 0z"/>
                                <path d="M9 6a3 3 0 1 1 0-6 3 3 0 0 1 0 6z"/>
                                <path
                                    d="M9 6h.5a2 2 0 0 1 1.983 1.738l3.11-1.382A1 1 0 0 1 16 7.269v7.462a1 1 0 0 1-1.406.913l-3.111-1.382A2 2 0 0 1 9.5 16H2a2 2 0 0 1-2-2V8a2 2 0 0 1 2-2h7z"/>
                            </Svg>
                            Каталог
                        </Nav.Link>
                        <Nav.Link onClick={props.onPlaylists}>
                            <Svg width={16} height={16} fill="currentColor"
                                 className="bi bi-binoculars-fill" viewBox="0 0 16 16">
                                <path
                                    d="M4.5 1A1.5 1.5 0 0 0 3 2.5V3h4v-.5A1.5 1.5 0 0 0 5.5 1h-1zM7 4v1h2V4h4v.882a.5.5 0 0 0 .276.447l.895.447A1.5 1.5 0 0 1 15 7.118V13H9v-1.5a.5.5 0 0 1 .146-.354l.854-.853V9.5a.5.5 0 0 0-.5-.5h-3a.5.5 0 0 0-.5.5v.793l.854.853A.5.5 0 0 1 7 11.5V13H1V7.118a1.5 1.5 0 0 1 .83-1.342l.894-.447A.5.5 0 0 0 3 4.882V4h4zM1 14v.5A1.5 1.5 0 0 0 2.5 16h3A1.5 1.5 0 0 0 7 14.5V14H1zm8 0v.5a1.5 1.5 0 0 0 1.5 1.5h3a1.5 1.5 0 0 0 1.5-1.5V14H9zm4-11H9v-.5A1.5 1.5 0 0 1 10.5 1h1A1.5 1.5 0 0 1 13 2.5V3z"/>
                            </Svg>
                            Подборки
                        </Nav.Link>
                        <Nav.Link onClick={props.onRooms}>
                            <Svg width={16} height={16} fill="currentColor"
                                 className="bi bi-people-fill" viewBox="0 0 16 16">
                                <path
                                    d="M7 14s-1 0-1-1 1-4 5-4 5 3 5 4-1 1-1 1H7Zm4-6a3 3 0 1 0 0-6 3 3 0 0 0 0 6Zm-5.784 6A2.238 2.238 0 0 1 5 13c0-1.355.68-2.75 1.936-3.72A6.325 6.325 0 0 0 5 9c-4 0-5 3-5 4s1 1 1 1h4.216ZM4.5 8a2.5 2.5 0 1 0 0-5 2.5 2.5 0 0 0 0 5Z"/>
                            </Svg>
                            Комнаты
                        </Nav.Link>
                        <Nav.Link onClick={props.onYouTube}>
                            <Svg width={16} height={16} fill="currentColor"
                                 className="bi bi-youtube" viewBox="0 0 16 16">
                                <path
                                    d="M8.051 1.999h.089c.822.003 4.987.033 6.11.335a2.01 2.01 0 0 1 1.415 1.42c.101.38.172.883.22 1.402l.01.104.022.26.008.104c.065.914.073 1.77.074 1.957v.075c-.001.194-.01 1.108-.082 2.06l-.008.105-.009.104c-.05.572-.124 1.14-.235 1.558a2.007 2.007 0 0 1-1.415 1.42c-1.16.312-5.569.334-6.18.335h-.142c-.309 0-1.587-.006-2.927-.052l-.17-.006-.087-.004-.171-.007-.171-.007c-1.11-.049-2.167-.128-2.654-.26a2.007 2.007 0 0 1-1.415-1.419c-.111-.417-.185-.986-.235-1.558L.09 9.82l-.008-.104A31.4 31.4 0 0 1 0 7.68v-.123c.002-.215.01-.958.064-1.778l.007-.103.003-.052.008-.104.022-.26.01-.104c.048-.519.119-1.023.22-1.402a2.007 2.007 0 0 1 1.415-1.42c.487-.13 1.544-.21 2.654-.26l.17-.007.172-.006.086-.003.171-.007A99.788 99.788 0 0 1 7.858 2h.193zM6.4 5.209v4.818l4.157-2.408L6.4 5.209z"/>
                            </Svg>
                            YouTube
                        </Nav.Link>
                    </Nav>
                    <Nav>
                        <Nav.Item>
                            <FilmSearch films={props.films} onFilmSearch={props.onFilmSearch} onClick={props.onFilm}/>
                        </Nav.Item>
                        {props.user &&
                            <>
                                <Nav.Item className="d-none d-xxl-flex align-items-center">
                                    <img src={props.user.avatarUrl} className={styles.avatar} alt="Аватар"/>
                                </Nav.Item>
                                <Nav.Link className="text-nowrap" onClick={props.onProfile} title="Профиль">
                                    <Svg width={16} height={16} fill="currentColor"
                                         className="bi bi-person-fill d-xxl-none" viewBox="0 0 16 16">
                                        <path
                                            d="M3 14s-1 0-1-1 1-4 6-4 6 3 6 4-1 1-1 1H3Zm5-6a3 3 0 1 0 0-6 3 3 0 0 0 0 6Z"/>
                                    </Svg>
                                    {props.user.name}
                                </Nav.Link>

                                {props.user.isInRole('admin') &&
                                    <NavDropdown title="Управление">
                                        <NavDropdown.Item>
                                            <Svg width={16} height={16} fill="currentColor"
                                                 className="bi bi-film" viewBox="0 0 16 16">
                                                <path
                                                    d="M0 1a1 1 0 0 1 1-1h14a1 1 0 0 1 1 1v14a1 1 0 0 1-1 1H1a1 1 0 0 1-1-1V1zm4 0v6h8V1H4zm8 8H4v6h8V9zM1 1v2h2V1H1zm2 3H1v2h2V4zM1 7v2h2V7H1zm2 3H1v2h2v-2zm-2 3v2h2v-2H1zM15 1h-2v2h2V1zm-2 3v2h2V4h-2zm2 3h-2v2h2V7zm-2 3v2h2v-2h-2zm2 3h-2v2h2v-2z"/>
                                            </Svg>
                                            Фильмы
                                        </NavDropdown.Item>
                                        <NavDropdown.Item>
                                            <Svg width={16} height={16} fill="currentColor"
                                                 className="bi bi-camera-video"
                                                 viewBox="0 0 16 16">
                                                <path fillRule="evenodd"
                                                      d="M0 5a2 2 0 0 1 2-2h7.5a2 2 0 0 1 1.983 1.738l3.11-1.382A1 1 0 0 1 16 4.269v7.462a1 1 0 0 1-1.406.913l-3.111-1.382A2 2 0 0 1 9.5 13H2a2 2 0 0 1-2-2V5zm11.5 5.175 3.5 1.556V4.269l-3.5 1.556v4.35zM2 4a1 1 0 0 0-1 1v6a1 1 0 0 0 1 1h7.5a1 1 0 0 0 1-1V5a1 1 0 0 0-1-1H2z"/>
                                            </Svg>
                                            Подборки
                                        </NavDropdown.Item>
                                    </NavDropdown>
                                }
                                <Nav.Link onClick={props.onLogout}>
                                    <Svg width={16} height={16} fill="currentColor"
                                         className="bi bi-box-arrow-right"
                                         viewBox="0 0 16 16">
                                        <path fillRule="evenodd"
                                              d="M10 12.5a.5.5 0 0 1-.5.5h-8a.5.5 0 0 1-.5-.5v-9a.5.5 0 0 1 .5-.5h8a.5.5 0 0 1 .5.5v2a.5.5 0 0 0 1 0v-2A1.5 1.5 0 0 0 9.5 2h-8A1.5 1.5 0 0 0 0 3.5v9A1.5 1.5 0 0 0 1.5 14h8a1.5 1.5 0 0 0 1.5-1.5v-2a.5.5 0 0 0-1 0v2z"/>
                                        <path fillRule="evenodd"
                                              d="M15.854 8.354a.5.5 0 0 0 0-.708l-3-3a.5.5 0 0 0-.708.708L14.293 7.5H5.5a.5.5 0 0 0 0 1h8.793l-2.147 2.146a.5.5 0 0 0 .708.708l3-3z"/>
                                    </Svg>
                                    Выход
                                </Nav.Link>
                            </>
                        }
                        {!props.user &&
                            <Nav.Link onClick={props.onLogin}>
                                <Svg width={16} height={16} fill="currentColor"
                                     className="bi bi-box-arrow-in-right" viewBox="0 0 16 16">
                                    <path fillRule="evenodd"
                                          d="M6 3.5a.5.5 0 0 1 .5-.5h8a.5.5 0 0 1 .5.5v9a.5.5 0 0 1-.5.5h-8a.5.5 0 0 1-.5-.5v-2a.5.5 0 0 0-1 0v2A1.5 1.5 0 0 0 6.5 14h8a1.5 1.5 0 0 0 1.5-1.5v-9A1.5 1.5 0 0 0 14.5 2h-8A1.5 1.5 0 0 0 5 3.5v2a.5.5 0 0 0 1 0v-2z"/>
                                    <path fillRule="evenodd"
                                          d="M11.854 8.354a.5.5 0 0 0 0-.708l-3-3a.5.5 0 1 0-.708.708L10.293 7.5H1.5a.5.5 0 0 0 0 1h8.793l-2.147 2.146a.5.5 0 0 0 .708.708l3-3z"/>
                                </Svg>
                                Вход
                            </Nav.Link>
                        }
                    </Nav>
                </NavbarBs.Collapse>
            </Container>
        </NavbarBs>
    );
};

export default Navbar;