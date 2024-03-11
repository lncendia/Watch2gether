import {useEffect} from 'react';
import {useNavigate} from "react-router-dom";
import {useInjection} from 'inversify-react';
import {IAuthService} from "../../services/AuthService/IAuthService.ts";

// Модуль silent аутентификации пользователя
const SignInSilentModule = () => {

    // Навигационный хук
    const navigate = useNavigate();
    const authService = useInjection<IAuthService>('AuthService');

    // Используем эффект
    useEffect(() => {
        authService.signInSilentCallback().then(() => navigate('/'))
    }, [authService, navigate])

    return (
        <div>
            Перенаправление...
        </div>
    )
};

export default SignInSilentModule;