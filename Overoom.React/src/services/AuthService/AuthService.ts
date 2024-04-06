import {UserManager} from 'oidc-client';
import {inject, injectable} from 'inversify';
import {IAuthService} from "./IAuthService.ts";

@injectable()
export class AuthService implements IAuthService {

    private readonly userManager: UserManager

    constructor(@inject('UserManager') userManager: UserManager) {
        this.userManager = userManager;
    }

    // Функция для получения Access токена
    public async getAccessToken(): Promise<string | null> {
        const user = await this.userManager.getUser();
        if (!user?.access_token) return null;
        return user.access_token;
    }

    // Функция для получения Id токена
    public async getIdToken(): Promise<string | null> {
        const user = await this.userManager.getUser();
        if (!user?.id_token) return null;
        return user.id_token;
    }

    // Функция для перенаправления на страницу аутентификации
    public async signIn(): Promise<void> {
        await this.userManager.signinRedirect();
    }

    // Функция для тихой аутентификации
    public async signInSilent(): Promise<void> {
        await this.userManager.signinSilent();
    }

    // Функция для обработки перенаправления после успешной аутентификации
    public async signInCallback(): Promise<void> {
        await this.userManager.signinRedirectCallback();
    }

    // Функция для обработки перенаправления после тихой аутентификации
    public async signInSilentCallback(): Promise<void> {
        await this.userManager.signinSilentCallback();
    }

    // Функция для перенаправления на страницу выхода из системы
    public async signOut(): Promise<void> {
        const user = await this.userManager.getUser();
        await this.userManager.signoutRedirect({id_token_hint: user?.id_token});
    }

    // Функция для обработки перенаправления после выхода из системы
    public async signOutCallback(): Promise<void> {
        await this.userManager.clearStaleState();
        await this.userManager.removeUser();
        await this.userManager.signoutRedirectCallback();
    }
}