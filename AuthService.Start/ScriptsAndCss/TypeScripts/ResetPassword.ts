/**
 * Класс функционала страницы восстановления пароля
 */
class ResetPassword {
    
    /**
     * Метод запускает функционал страницы восстановления пароля
     */
    startResetPassword() {

        // получаем все поля ввода с классом .input100
        document.querySelectorAll('.wrap-input input').forEach(element => {

            // добавляем обработчик события потери фокуса
            element.addEventListener('blur', ev => this.blur((ev.currentTarget as HTMLInputElement)));
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