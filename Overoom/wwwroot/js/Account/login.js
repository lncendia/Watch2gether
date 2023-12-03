$(document).ready(startLogin)

function startLogin() {

    // получаем все поля ввода с классом .input100
    document.querySelectorAll('.wrap-input input').forEach(element => {

        // добавляем обработчик события потери фокуса
        element.addEventListener('blur', ev => blurHandler(ev.currentTarget));

    });

    // Флаг для переключения видимости пароля
    let showPass = false;

    // получаем тег span иконки переключателя видимости пароля и добавляем ей обработчик клика
    document.querySelector('.btn-show-pass').addEventListener('click', function () {

        // если false
        if (!showPass) {

            // меняем значение атрибута type на text для поля ввода пароля
            this.nextElementSibling.setAttribute('type', 'text');

            // убираем иконку глаза
            this.querySelector('i').classList.remove('zmdi-eye');

            // добавляем иконку зачеркнутого глаза
            this.querySelector('i').classList.add('zmdi-eye-off');

            // меняем флаг
            showPass = true;

        } else {

            // меняем значение атрибута type на password для поля ввода пароля
            this.nextElementSibling.setAttribute('type', 'password');

            // добавляем иконку глаза
            this.querySelector('i').classList.add('zmdi-eye');

            // убираем иконку зачеркнутого глаза
            this.querySelector('i').classList.remove('zmdi-eye-off');

            // меняем флаг
            showPass = false;
        }
    });

}

/**
 * Метод реагирует на потерю фокуса
 */
function blurHandler(element) {

    // если значение не пустое
    if (element.value.trim() !== "") {

        // добавляем класс 'has-val'
        element.classList.add('has-val');
    } else {

        // удаляем класс 'has-val'
        element.classList.remove('has-val');
    }
}
    