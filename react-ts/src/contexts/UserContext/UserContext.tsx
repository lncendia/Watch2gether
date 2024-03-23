import React, {createContext, useContext, useState, useEffect, ReactNode} from 'react';
import {User, UserManager} from 'oidc-client';
import {useInjection} from "inversify-react";
import {IAuthorizedUser} from "./AuthorizedUser.ts";


// Создайте интерфейс для контекста
interface UserContextType {
    authorizedUser: IAuthorizedUser | null;
}

// Создайте сам контекст
const UserContext = createContext<UserContextType | undefined>(undefined);

// Создайте провайдер
interface UserContextProviderProps {
    children: ReactNode;
}

export const UserContextProvider: React.FC<UserContextProviderProps> = ({children}) => {

    const [authorizedUser, setIAuthorizedUser] = useState<IAuthorizedUser | null>(null);

    const userManager = useInjection<UserManager>('UserManager')

    useEffect(() => {
        const onUserLoaded = (user: User) => {

            // Обновить текущего пользователя
            setIAuthorizedUser(mapUser(user));

            setTimeout(() => {
                console.log("Устанавливаю")
                setIAuthorizedUser(prev => {
                    return {...prev!}
                });
            }, 10000)
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
    }, [userManager]);

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

function mapUser(user: User): IAuthorizedUser {

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

    return {
        avatarUrl: profilePhoto,
        email: user.profile.email!,
        id: user.profile.sub,
        locale: user.profile.locale!,
        name: user.profile.name!,
        roles: roles
    }
}