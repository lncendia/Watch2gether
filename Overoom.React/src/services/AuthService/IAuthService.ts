export interface IAuthService {

    // Функция для перенаправления на страницу аутентификации
    signIn(): Promise<void>;

    // Функция для тихой аутентификации
    signInSilent(): Promise<void>;

    // Функция для обработки перенаправления после успешной аутентификации
    signInCallback(): Promise<void>;

    // Функция для обработки перенаправления после тихой аутентификации
    signInSilentCallback(): Promise<void>;

    // Функция для перенаправления на страницу выхода из системы
    signOut(): Promise<void>;

    // Функция для обработки перенаправления после выхода из системы
    signOutCallback(): Promise<void>;

    // Функция для получения Access токена
    getAccessToken(): Promise<string | null>;

    // Функция для получения Id токена
    getIdToken(): Promise<string | null>;
}