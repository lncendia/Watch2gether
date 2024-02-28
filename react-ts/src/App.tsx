import {createBrowserRouter, RouterProvider} from "react-router-dom"
import SignInPage from "./pages/SignInPage/SignInPage"
import LayoutPage from "./pages/LayoutPage/LayoutPage"
import SignOutPage from "./pages/SignOutPage/SignOutPage"
import SignInSilentPage from "./pages/SignInSilentPage/SignInSilentPage.tsx";
import FilmPage from "./pages/FilmPage/FilmPage.tsx"
import 'bootstrap/dist/css/bootstrap.min.css';
import HomePage from "./pages/HomePage/HomePage.tsx";
import './index.css'

// Основной класс приложения
const App = () => {

    // Создаем объект BrowserRouter для маршрутизации страниц
    const router = createBrowserRouter([
        {
            path: "/",
            element: <HomePage/>,
        },
        {
            // Основой элемент - шаблонный 
            element: <LayoutPage/>,

            // Массив дочерних элементов
            children: [
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
                    path: '/films',
                    element: <FilmPage/>
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
