import {InputWrapper} from "../helpers/InputWrapper";
import {EmailCodeHandler} from "../helpers/EmailCodeHandler";

/**
 * Класс функционала страницы входа 2фа
 */
export class LoginTwoStep {
    
    emailHandler: EmailCodeHandler = new EmailCodeHandler();

    /**
     * Метод запускает функционал страницы входа 2фа
     */
    startLoginTwoStep() {

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

    /**
     * Метод реагирует на потерю фокуса
     */
    blur(element: HTMLInputElement) {

        // если значение не пустое
        if (element.value.trim() != "") {

            // добавляем класс 'has-val'
            element.classList.add('has-val');
        } else {

            // удаляем класс 'has-val'
            element.classList.remove('has-val');
        }
    }
}