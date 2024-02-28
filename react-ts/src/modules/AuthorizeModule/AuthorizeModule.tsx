import {useNavigate} from "react-router-dom";
import {ReactNode, useEffect, useState} from "react";
import NotEnoughRights from "../../components/NotEnoughRights/NotEnoughRights.tsx";
import {useApiService} from "../../contexts/ApiContext.tsx";

// Модуль проверки авторизации пользователя
const AuthorizeModule = ({role, children}: { role?: string, children: ReactNode }) => {

    // Навигационный хук
    const navigate = useNavigate();
    const {authorizedUser} = useApiService();

    // Состояние показа дочерних компонент переданных через props
    const [show, setShow] = useState(false);

    // Метод возвращает пользователя на 1 страницу назад
    const goBack = () => {

        // Возвращаем пользователю назад
        navigate(-1);
    }

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
    return <NotEnoughRights goBack={goBack}/>

}

export default AuthorizeModule;