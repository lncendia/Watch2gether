/**
 * Класс функционала страницы установки нового пароля
 */
class NewPassword {

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

        // получаем все поля ввода с классом .input100
        document.querySelectorAll('.wrap-input input').forEach(element => {

            // добавляем обработчик события потери фокуса
            element.addEventListener('blur', ev => this.blur((ev.currentTarget as HTMLInputElement)));
        });

        // Флаг для переключения видимости пароля
        let showPass: boolean = false;
        
        // Флаг для переключения видимости подтверждения пароля
        let showPassConfirm: boolean = false;
        
        // получаем тег span иконки переключателя видимости пароля и добавляем ей обработчик клика
        document.querySelector('#show-pass').addEventListener('click', ev=>{
            showPass = this.toggleVisible(ev.currentTarget as HTMLSpanElement, showPass);
        });

        // получаем тег span иконки переключателя видимости подтверждения пароля и добавляем ей обработчик клика
        document.querySelector('#show-pass-confirm').addEventListener('click', ev=>{
            showPassConfirm = this.toggleVisible(ev.currentTarget as HTMLSpanElement, showPassConfirm);
        });

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