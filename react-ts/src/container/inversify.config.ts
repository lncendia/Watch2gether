import {Container} from "inversify";
import {UserManager} from 'oidc-client';
import {UserManagerSettings, WebStorageStateStore} from 'oidc-client';
import {AuthService} from "../services/AuthService/AuthService.ts";
import {FilmService} from "../services/FilmsService/FilmService.ts";
import axios from "axios";
import {IAuthService} from "../services/AuthService/IAuthService.ts";
import {IFilmService} from "../services/FilmsService/IFilmService.ts";


const config: UserManagerSettings = {

    // URL-адрес OpenID Connect провайдера
    authority: "https://localhost:10001",

    // Идентификатор клиента
    client_id: "client_react",

    // URI перенаправления после успешной аутентификации
    redirect_uri: "https://localhost:5173/signin-oidc",

    // Тип ответа при аутентификации
    response_type: "code",

    // Запрашиваемые области доступа
    scope: "openid profile email roles Business",

    // URI перенаправления после выхода из системы
    post_logout_redirect_uri: "https://localhost:5173/signout-oidc",

    // Флаг автоматического тихого обновления токена доступа
    automaticSilentRenew: true,

    // URI перенаправления для тихого обновления токена доступа
    silent_redirect_uri: "https://localhost:5173/signin-silent-oidc",

    // Хранилище состояния пользователя
    userStore: new WebStorageStateStore({store: localStorage}),
};

// Создаем контейнер
const container = new Container();

const filmAxios = axios.create({
    baseURL: 'https://localhost:7131/filmApi/films'
});


container.bind<UserManager>('UserManager')
    .toDynamicValue(() => new UserManager(config))
    .inSingletonScope();

container.bind<IAuthService>('AuthService')
    .to(AuthService)
    .inSingletonScope();

container.bind<IFilmService>('FilmService')
    .toDynamicValue(() => new FilmService(filmAxios))
    .inSingletonScope();


export default container;


// function configureAxiosAuthorization(axiosInstance: AxiosInstance, container: Container): void {
//     axiosInstance.interceptors.request.use(async config => {
//         const userManager = container.get<UserManager>('UserManager');
//         const user = await userManager.getUser();
//         if (user && user.access_token) {
//             config.headers.Authorization = `Bearer ${user.access_token}`;
//         }
//         return config;
//     });
// }