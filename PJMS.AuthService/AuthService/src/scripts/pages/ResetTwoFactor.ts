import {InputWrapper} from "../helpers/InputWrapper";
import {EmailCodeHandler} from "../helpers/EmailCodeHandler";

/**
 * Класс функционала страницы входа 2фа
 */
export class ResetTwoFactor {

    emailHandler: EmailCodeHandler = new EmailCodeHandler();

    /**
     * Метод запускает функционал страницы входа 2фа
     */
    startResetTwoStep() {

        // получаем все поля ввода с классом .wrap-input input
        new InputWrapper('.wrap-input input');

        // получаем ссылку на метод отправки email
        const requestLink = (document.querySelector('#request-email-link') as HTMLLinkElement).href;

        // удаляем путь из атрибута href, чтобы не срабатывал переход по нажатию
        (document.querySelector('#request-email-link') as HTMLLinkElement).href = '#';

        // отлавливаем нажатие на ссылку
        document.querySelector('#request-email-link').addEventListener('click', () => {

            // обрабатываем запрос
            this.emailHandler.processRequest(requestLink);
        });
    }
}