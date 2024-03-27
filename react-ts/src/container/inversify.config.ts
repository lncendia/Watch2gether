import {Container} from "inversify";
import {UserManager} from 'oidc-client';
import {UserManagerSettings, WebStorageStateStore} from 'oidc-client';
import {AuthService} from "../services/AuthService/AuthService.ts";
import {FilmsService} from "../services/FilmsService/FilmsService.ts";
import axios, {AxiosInstance} from "axios";
import {IAuthService} from "../services/AuthService/IAuthService.ts";
import {IFilmsService} from "../services/FilmsService/IFilmsService.ts";
import {CommentsService} from "../services/CommentsService/CommentsService.ts";
import {ICommentsService} from "../services/CommentsService/ICommentsService.ts";
import {IProfileService} from "../services/ProfileService/IProfileService.ts";
import {ProfileService} from "../services/ProfileService/ProfileService.ts";
import {IPlaylistsService} from "../services/PlaylistsService/IPlaylistsService.ts";
import {PlaylistsService} from "../services/PlaylistsService/PlaylistsService.ts";
import {IFilmRoomsService} from "../services/RoomsService/IFilmRoomsService.ts";
import {FilmRoomsService} from "../services/RoomsService/FilmRoomsService.ts";
import {IYoutubeRoomsService} from "../services/RoomsService/IYoutubeRoomsService.ts";
import {YoutubeRoomsService} from "../services/RoomsService/YoutubeRoomsService.ts";
import {IFilmRoomServiceFactory} from "../services/FilmRoomService/IFilmRoomServiceFactory.ts";
import {FilmRoomServiceFactory} from "../services/FilmRoomService/FilmRoomServiceFactory.ts";


const config: UserManagerSettings = {

    // URL-адрес OpenID Connect провайдера
    authority: "https://localhost:10001",

    // Идентификатор клиента
    client_id: "overoom_react",

    // URI перенаправления после успешной аутентификации
    redirect_uri: "https://localhost:5173/signin-oidc",

    // Тип ответа при аутентификации
    response_type: "code",

    // Запрашиваемые области доступа
    scope: "openid profile roles Films Rooms",

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

const axiosInstance = axios.create({
    baseURL: 'https://localhost:7131/filmApi/'
});


container.bind<UserManager>('UserManager')
    .toDynamicValue(() => new UserManager(config))
    .inSingletonScope();

container.bind<IAuthService>('AuthService')
    .to(AuthService)
    .inSingletonScope();

container.bind<IFilmsService>('FilmsService')
    .toDynamicValue(() => new FilmsService(axiosInstance))
    .inSingletonScope();

container.bind<IProfileService>('ProfileService')
    .toDynamicValue(() => new ProfileService(axiosInstance))
    .inSingletonScope();

container.bind<ICommentsService>('CommentsService')
    .toDynamicValue(() => new CommentsService(axiosInstance, config.authority!))
    .inSingletonScope();

container.bind<IPlaylistsService>('PlaylistsService')
    .toDynamicValue(() => new PlaylistsService(axiosInstance))
    .inSingletonScope();

container.bind<IFilmRoomsService>('FilmRoomsService')
    .toDynamicValue(() => new FilmRoomsService(axiosInstance))
    .inSingletonScope();

container.bind<IYoutubeRoomsService>('YoutubeRoomsService')
    .toDynamicValue(() => new YoutubeRoomsService(axiosInstance))
    .inSingletonScope();

container.bind<IFilmRoomServiceFactory>('FilmRoomServiceFactory')
    .toDynamicValue(() => new FilmRoomServiceFactory(() => tokenFactory(container), config.authority!))
    .inSingletonScope();

configureAxiosAuthorization(axiosInstance, container)

export default container;

async function tokenFactory(container: Container): Promise<string> {
    const userManager = container.get<UserManager>('UserManager');
    const user = await userManager.getUser();
    if (user && user.access_token) {
        return user.access_token
    }
    return ""
}

function configureAxiosAuthorization(axiosInstance: AxiosInstance, container: Container): void {
    axiosInstance.interceptors.request.use(async config => {
        const userManager = container.get<UserManager>('UserManager');
        const user = await userManager.getUser();
        if (user && user.access_token) {
            config.headers.Authorization = `Bearer ${user.access_token}`;
        }
        return config;
    });
}