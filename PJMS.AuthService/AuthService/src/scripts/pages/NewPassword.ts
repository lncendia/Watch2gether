import {InputWrapper} from "../helpers/InputWrapper";
import {PasswordHide} from "../helpers/PasswordHide";
import {PasswordStrengthValidator} from "../helpers/PasswordStrengthValidator";

/**
 * Класс функционала страницы установки нового пароля
 */
export class NewPassword {

    /**
     * Валидатор надежности пароля на странице
     */
    validator: PasswordStrengthValidator = new PasswordStrengthValidator(
        document.querySelector('#strength-valid') as HTMLDivElement,
        document.querySelector('#new-pass') as HTMLFormElement
    )

    /**
     * Метод запускает функционал страницы установки нового пароля
     */
    startNewPassword() {

        // получаем все поля ввода с классом .wrap-input input
        new InputWrapper('.wrap-input input');

        // получаем тег span иконки переключателя видимости пароля и добавляем ей обработчик клика
        new PasswordHide('#show-pass');

        // получаем тег span иконки переключателя видимости подтверждения пароля и добавляем ей обработчик клика
        new PasswordHide('#show-pass-confirm');

        // получаем поле для ввода пароля и добавляем обработчик изменения текста
        document.querySelector('#NewPassword').addEventListener('input', ev =>
            this.validator.checkPasswordStrength((ev.currentTarget as HTMLInputElement).value)
        );

        // получаем форму при отправке для проверки надежности пароля
        document.querySelector('form#new-pass').addEventListener('submit', ev => {
            ev.preventDefault();
            //Передаем id input элемента с паролем
            this.validator.validateFormPassword('NewPassword')
        })
    }
}