import {PasswordStrengthValidator} from "../helpers/PasswordStrengthValidator";
import {InputWrapper} from "../helpers/InputWrapper";
import {PasswordHide} from "../helpers/PasswordHide";

/**
 * Класс функционала страницы настроек
 */
export class Settings {

    /**
     * Валидатор надежности пароля на странице
     */
    validator: PasswordStrengthValidator = new PasswordStrengthValidator(
        document.querySelector('#strength-valid') as HTMLDivElement,
        document.querySelector('form#change-pass') as HTMLFormElement
    );
    
    /**
     * Метод запускает функционал страницы настроек
     */
    startSettings() {

        // Получаем все элементы с классом "disabled"
        let links = document.querySelectorAll(".disabled");

        // Добавляем обработчик события "click" для каждого элемента, чтоб ссылки были неактивны
        links.forEach(l => l.addEventListener("click", ev => ev.preventDefault()));

        // Получаем элемент с идентификатором "exampleModal"
        const exampleModal = document.getElementById('exampleModal');

        // Добавляем обработчик события "show.bs.modal"
        exampleModal.addEventListener('show.bs.modal', event => {

            // Получаем кнопку, которая вызвала модальное окно
            const button = (event as MouseEvent).relatedTarget as HTMLElement;

            // Получаем значение атрибута "data-bs-provider" кнопки
            const provider = button.getAttribute('data-bs-provider');

            // Получаем элемент с классом "modal-body" внутри модального окна
            const modalBody = exampleModal.querySelector('.modal-body');

            // Удаляем последнее слово из текста внутри "modal-body", если есть знак вопроса
            modalBody.textContent = `${this.deleteLastWord(modalBody.textContent)} ${provider}?`;

            // Получаем ссылку с классом "modal-footer a" внутри модального окна
            const linkElement = exampleModal.querySelector('.modal-footer a') as HTMLLinkElement;

            // Устанавливаем значение href для ссылки
            linkElement.href = `/Settings/RemoveLogin?provider=${provider}`;
        });

        // получаем все поля ввода с классом .wrap-input input
        new InputWrapper('.wrap-input input');

        // получаем тег span иконки переключателя видимости старого пароля и добавляем ей обработчик клика
        new PasswordHide('#show-old-pass');
        
        // получаем тег span иконки переключателя видимости нрвого пароля и добавляем ей обработчик клика
        new PasswordHide('#show-new-pass');

        // получаем тег span иконки переключателя видимости подтверждения нового пароля и добавляем ей обработчик клика
        new PasswordHide('#show-new-pass-confirm');

        // получаем поле для ввода пароля и добавляем обработчик изменения текста
        document.querySelector('#NewPassword').addEventListener('input', ev => 
            this.validator.checkPasswordStrength((ev.currentTarget as HTMLInputElement).value)
        );

        // получаем форму при отправке для проверки надежности пароля
        document.querySelector('form#change-pass').addEventListener('submit', ev => {
            ev.preventDefault();
            //Передаем id input элемента с паролем
            this.validator.validateFormPassword('NewPassword');
        })
    }

    /**
     * Метод удаляет название провайдера, если оно указано
     */
    deleteLastWord(str: string): string {

        // Проверяем, содержит ли строка знак вопроса
        if (str.includes('?')) {

            // Разбиваем строку на слова
            const words = str.split(' ');

            // Удаляем последнее слово
            words.pop();

            // Объединяем оставшиеся слова обратно в строку
            return words.join(' ');
        }

        // Возвращаем исходную строку без изменений
        return str;
    }
}