/**
 * Класс функционала страницы настроек
 */
class Settings {

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
            modalBody.textContent = `${this.DeleteLastWord(modalBody.textContent)} ${provider}?`;

            // Получаем ссылку с классом "modal-footer a" внутри модального окна
            const linkElement = exampleModal.querySelector('.modal-footer a') as HTMLLinkElement;

            // Получаем значение "returnUrl" из элемента input внутри модального окна
            const returnUrl = (document.querySelector(".modal input") as HTMLInputElement).value;

            // Устанавливаем значение href для ссылки
            linkElement.href = `/Settings/RemoveLogin?provider=${provider}&returnUrl=${returnUrl}`;
        });

        // получаем все поля ввода с классом .input100
        document.querySelectorAll('.wrap-input input').forEach(element => {

            // добавляем обработчик события потери фокуса
            element.addEventListener('blur', ev => this.blur((ev.currentTarget as HTMLInputElement)));
        });
        
        // Флаг для переключения видимости старого пароля
        let showOldPass: boolean = false;

        // Флаг для переключения видимости нового пароля
        let showNewPass: boolean = false;

        // Флаг для переключения видимости подтверждения нового пароля
        let showNewPassConfirm: boolean = false;

        // получаем тег span иконки переключателя видимости старого пароля и добавляем ей обработчик клика
        document.querySelector('#show-old-pass')?.addEventListener('click', ev => {
            showOldPass = this.toggleVisible(ev.currentTarget as HTMLSpanElement, showOldPass);
        });
        
        // получаем тег span иконки переключателя видимости нрвого пароля и добавляем ей обработчик клика
        document.querySelector('#show-new-pass').addEventListener('click', ev => {
            showNewPass = this.toggleVisible(ev.currentTarget as HTMLSpanElement, showNewPass);
        });

        // получаем тег span иконки переключателя видимости подтверждения нового пароля и добавляем ей обработчик клика
        document.querySelector('#show-new-pass-confirm').addEventListener('click', ev => {
            showNewPassConfirm = this.toggleVisible(ev.currentTarget as HTMLSpanElement, showNewPassConfirm);
        });

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
    DeleteLastWord(str: string): string {

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

    /**
     * Метод переключает режим видимости поля
     */
    toggleVisible(element: HTMLSpanElement, isShoved: boolean): boolean {

        // если false
        if (!isShoved) {

            // меняем значение атрибута type на text для поля ввода пароля
            element.nextElementSibling.setAttribute('type', 'text');

            // убираем иконку глаза
            element.querySelector('i').classList.remove('zmdi-eye');

            // добавляем иконку зачеркнутого глаза
            element.querySelector('i').classList.add('zmdi-eye-off');

        } else {

            // меняем значение атрибута type на password для поля ввода пароля
            element.nextElementSibling.setAttribute('type', 'password');

            // добавляем иконку глаза
            element.querySelector('i').classList.add('zmdi-eye');

            // убираем иконку зачеркнутого глаза
            element.querySelector('i').classList.remove('zmdi-eye-off');
        }

        return !isShoved;
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