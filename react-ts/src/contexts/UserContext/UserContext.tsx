import React, {createContext, useContext, useState, useEffect, ReactNode} from 'react';
import {User, UserManager} from 'oidc-client';
import {AuthorizedUser} from "./AuthorizedUser.ts";
import {useInjection} from "inversify-react";


// Создайте интерфейс для контекста
interface UserContextType {
    authorizedUser: AuthorizedUser | null;
}

// Создайте сам контекст
const UserContext = createContext<UserContextType | undefined>(undefined);

// Создайте провайдер
interface UserContextProviderProps {
    children: ReactNode;
}

export const UserContextProvider: React.FC<UserContextProviderProps> = ({children}) => {

    const [authorizedUser, setAuthorizedUser] = useState<AuthorizedUser | null>(null);

    const userManager = useInjection<UserManager>('UserManager')

    useEffect(() => {
        const onUserLoaded = (user: User) => {

            // Обновить текущего пользователя
            setAuthorizedUser(mapUser(user));
        };

        // Подписаться на события обновления пользователя
        userManager.events.addUserLoaded(onUserLoaded);

        userManager.getUser().then(user => {
            if (!user) {
                return
            } else if (user.expired) {
                userManager.signinSilent().then();
            } else {
                userManager.events.load(user);
            }
        })

        return () => {
            // Очистка подписки
            userManager.events.removeUserLoaded(onUserLoaded);
        };
    }, []);

    return (
        <UserContext.Provider value={{authorizedUser}}>
            {children}
        </UserContext.Provider>
    );
}

// Хук для использования контекста
export const useUser = () => {
    const context = useContext(UserContext);
    if (context === undefined) {
        throw new Error('useUser must be used within a UserContextProvider');
    }
    return context;
};

function mapUser(user: User): AuthorizedUser {

    // Получаем claims пользователя
    const userClaims = user.profile as any;

    // Получем роли пользователя
    const userRoles = userClaims.role

    let roles: string[] = [];

    // Если роли не установлены - возвращаем false
    if (userRoles) {

        // Если claim роли является строкой сравниваем указанную роль с ролью пользователя
        if (typeof userRoles === 'string') roles = [userRoles]

        // Если нет
        else {

            // Конвертируем список ролей в массив
            roles = userRoles as Array<string>;
        }
    }

    let profilePhoto: string;

    if (user.profile.picture) {
        profilePhoto = `https://localhost:10001/${user.profile.picture}`
    } else {
        profilePhoto = '/vite.svg'
    }

    return new AuthorizedUser(user.profile.sub, user.profile.name!, profilePhoto, roles, user.profile.email!, user.profile.locale!);
}