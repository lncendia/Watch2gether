import {useNavigate} from "react-router-dom";
import {ReactNode, useCallback, useEffect, useState} from "react";
import NotEnoughRights from "../../components/NotEnoughRights/NotEnoughRights.tsx";
import {useUser} from "../../contexts/UserContext/UserContext.tsx";

// Модуль проверки авторизации пользователя
const AuthorizeModule = ({role, children, showError}: { role?: string, children: ReactNode, showError: boolean }) => {

    // Навигационный хук
    const navigate = useNavigate();
    const {authorizedUser} = useUser();

    // Состояние показа дочерних компонент переданных через props
    const [show, setShow] = useState(false);

    // Метод возвращает пользователя на 1 страницу назад
    const goBack = useCallback(() => navigate(-1), [navigate])

    // Применяем эффект при изменении роли
    useEffect(() => {

        // Если не получили пользователя возвращаем false
        if (!authorizedUser) setShow(false);

        // Если роль не была указана возвращаем true
        else if (!role) setShow(true);

        // Проверяем, содержит ли массив нужную роль
        else setShow(authorizedUser.roles.includes(role));

    }, [authorizedUser, role]);

    // Если состояние true - рендерим дочерние компоненты
    if (show) return children;

    // Если нет - выводим компонент недостаточной авторизации
    if (showError) return <NotEnoughRights goBack={goBack}/>

    return <></>

}

export default AuthorizeModule;