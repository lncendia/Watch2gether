/**
 * Класс добавляет функциональность для просмотра введенного пароля
 */
export class PasswordHide {
    
    /**
     * Конструктор
     */
    constructor(selector: string) {

        // Флаг для переключения видимости пароля
        let showPass: boolean = false;

        // получаем тег span иконки переключателя видимости пароля и добавляем ей обработчик клика
        document.querySelector(selector).addEventListener('click', function (this: HTMLSpanElement) {

            // если false
            if (!showPass) {

                // меняем значение атрибута type на text для поля ввода пароля
                this.nextElementSibling.setAttribute('type', 'text');

                // убираем иконку глаза
                this.querySelector('i').classList.remove('zmdi-eye');

                // добавляем иконку зачеркнутого глаза
                this.querySelector('i').classList.add('zmdi-eye-off');

            } else {

                // меняем значение атрибута type на password для поля ввода пароля
                this.nextElementSibling.setAttribute('type', 'password');

                // добавляем иконку глаза
                this.querySelector('i').classList.add('zmdi-eye');

                // убираем иконку зачеркнутого глаза
                this.querySelector('i').classList.remove('zmdi-eye-off');
            }

            // меняем флаг
            showPass = !showPass
        });
    }
}