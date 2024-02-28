import { useInjection } from "inversify-react";
import {ButtonContent} from "../../components/ButtonsPanel/ButtonContent.ts";
import ButtonsPanel from "../../components/ButtonsPanel/ButtonsPanel.tsx";
import {IAuthService} from "../../services/AuthService/IAuthService.ts";

// Компонент основной панели с кнопками
const ManageModule = () => {

    const authService = useInjection<IAuthService>('AuthService');

    // Массив содержимого кнопок
    const buttons: ButtonContent[] = [
        {
            title: 'Войти',
            action: authService.signIn.bind(authService)
        },
        {
            title: 'Выйти',
            action: authService.signOut.bind(authService)
        },
        {
            title: 'Получить Access Token',
            action: async () => {
                console.log((await authService.getAccessToken()))
            }
        },
        {
            title: 'Получить Id Token',
            action: async () => {
                console.log((await authService.getIdToken()))
            }
        },
        {
            title: 'Тихое обновление',
            action: authService.signInSilent.bind(authService)
        }
    ]

    // Вовзращаем панель с кнопками
    return (
        <ButtonsPanel buttons={buttons}/>
    );
};

export default ManageModule;