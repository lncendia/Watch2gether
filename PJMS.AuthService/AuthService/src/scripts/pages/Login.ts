import {InputWrapper} from "../helpers/InputWrapper";
import {PasswordHide} from "../helpers/PasswordHide";

/**
 * Класс функционала страницы авторизации
 */
export class Login {

    /**
     * Метод запускает функционал страницы авторизации
     */
    startAccount() {

        // получаем все поля ввода с классом .wrap-input input
        new InputWrapper('.wrap-input input');

        // получаем тег span иконки переключателя видимости пароля и добавляем ей обработчик клика
        new PasswordHide('.btn-show-pass');
    }
}