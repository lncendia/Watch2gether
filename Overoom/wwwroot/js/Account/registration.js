$(document).ready(startRegistration)

function startRegistration() {

    // получаем все поля ввода с классом .input100
    document.querySelectorAll('.wrap-input input').forEach(element => {

        // добавляем обработчик события потери фокуса
        element.addEventListener('blur', ev => blurHandler(ev.currentTarget));

    });

    // Флаг для переключения видимости пароля
    let showPass = false;

    // Флаг для переключения видимости пароля
    let showPassConfirm = false;

    // получаем тег span иконки переключателя видимости пароля и добавляем ей обработчик клика
    document.querySelector('#show-pass').addEventListener('click', ev => {
        showPass = toggleVisible(ev.currentTarget, showPass);
    });

    // получаем тег span иконки переключателя видимости подтверждения пароля и добавляем ей обработчик клика
    document.querySelector('#show-pass-confirm').addEventListener('click', ev => {
        showPassConfirm = toggleVisible(ev.currentTarget, showPassConfirm);
    });

}

// Метод переключает режим видимости поля
function toggleVisible(element, isShoved) {

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

//Метод реагирует на потерю фокуса
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