import {createBrowserRouter, RouterProvider} from "react-router-dom"
import SignInPage from "./pages/SignInPage/SignInPage"
import LayoutPage from "./pages/LayoutPage/LayoutPage"
import SignOutPage from "./pages/SignOutPage/SignOutPage"
import SignInSilentPage from "./pages/SignInSilentPage/SignInSilentPage.tsx";
import CatalogPage from "./pages/CatalogPage/CatalogPage.tsx"
import 'bootstrap/dist/css/bootstrap.min.css';
import HomePage from "./pages/HomePage/HomePage.tsx";
import './index.css'
import FilmPage from "./pages/FilmPage/FilmPage.tsx";
import ProfilePage from "./pages/ProfilePage/ProfilePage.tsx";
import SearchPage from "./pages/SearchPage/SearchPage.tsx";
import PlaylistsPage from "./pages/PlaylistsPage/PlaylistsPage.tsx";
import PlaylistPage from "./pages/PlaylistPage/PlaylistPage.tsx";
import FilmRoomPage from "./pages/FilmRoomPage/FilmRoomPage.tsx";

// Основной класс приложения
const App = () => {

    // Создаем объект BrowserRouter для маршрутизации страниц
    const router = createBrowserRouter([
        {
            path: "/",
            element: <HomePage/>
        },
        {
            path: '/signin-oidc',
            element: <SignInPage/>
        },
        {
            path: '/signin-silent-oidc',
            element: <SignInSilentPage/>
        },
        {
            path: '/signout-oidc',
            element: <SignOutPage/>
        },
        {
            // Основой элемент - шаблонный 
            element: <LayoutPage/>,

            // Массив дочерних элементов
            children: [
                {
                    path: '/catalog',
                    element: <CatalogPage/>
                },
                {
                    path: '/playlists',
                    element: <PlaylistsPage/>
                },
                {
                    path: '/playlist',
                    element: <PlaylistPage/>
                },
                {
                    path: '/search',
                    element: <SearchPage/>
                },
                {
                    path: '/film',
                    element: <FilmPage/>
                },
                {
                    path: '/profile',
                    element: <ProfilePage/>
                },
                {
                    path: '/filmRoom',
                    element: <FilmRoomPage/>
                },
            ]
        }
    ])

    // Возвращаем маршрутизированные страницы
    return (
        <RouterProvider router={router}/>
    )
}

export default App
