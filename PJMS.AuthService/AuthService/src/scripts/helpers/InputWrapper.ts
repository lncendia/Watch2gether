/**
 * Класс делает input функциональными
 */
export class InputWrapper {

    /**
     * Конструктор
     */
    constructor(selector: string) {

        // получаем все поля ввода по селектору
        document.querySelectorAll(selector).forEach(element => {

            // добавляем обработчик события потери фокуса
            element.addEventListener('blur', ev => this.blur((ev.currentTarget as HTMLInputElement)));
        });
    }

    /**
     * Метод реагирует на потерю фокуса
     */
    blur(element: HTMLInputElement){

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