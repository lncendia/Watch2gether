/**
 * Класс добавляет функциональность запроса кода 2fa на почту
 */
export class EmailCodeHandler {

    /**
     * Оработчик запроса
     */
    async processRequest(requestLink: string) {

        // Отправляем запрос
        let response = await fetch(requestLink);

        // Получаем элемент сообщения об ошибке
        let error = document.querySelector('#request-email-error') as HTMLDivElement;

        // Получаем элемент кнопки отправки сообщения
        let link = document.querySelector('#request-email-link') as HTMLLinkElement;

        // Меняем сообщение на кнопке
        link.innerHTML = document.querySelector('#resending-msg').innerHTML

        // Если пришел ответ со статусом OK
        if (response.ok) {

            // Значение для таймера в секундах
            const duration: number = 60;

            // Получаем элемент сообщения для успешной отправки
            let success = document.querySelector('#request-email-success') as HTMLDivElement;

            // Получаем элемент, показывающий таймер
            let timer = success.querySelector('span') as HTMLSpanElement;

            // Отключаем кнопку отправки запроса
            link.classList.add('disabled');

            // Делаем кнопку серой
            link.setAttribute('style', 'background: gray');

            // Скрываем сообщение об ошибке
            error.setAttribute('hidden', 'hidden')

            // Показываем сообщение о возможности повторной отправки через 30 секунд
            success.removeAttribute('hidden');

            // Запускаем таймер на возможность повторного запроса
            this.startTimer(duration, timer)

            // Ставим таймер
            setTimeout(() => {

                // После таймера включаем кнопку отправки запроса
                link.classList.remove('disabled');

                // Возвращаем кнопке цвет
                link.removeAttribute('style');

                // Скрываем сообщение о возможности повторной отправки
                success.setAttribute('hidden', 'hidden');

                // Ставим задержку на таймаут
            }, duration * 1000);

            // Если пришел код ответа отличный от 200
        } else {

            // Показываем сообщение об ошибке
            error.removeAttribute('hidden');
        }
    }

    /**
     * Метод отображающий таймер на странице
     */
    startTimer(duration: number, displayElement: HTMLElement) {

        // Ставим интервал для таймера
        let timerId = setInterval(() => {

            // Вычисляем целое количество минут
            const minutes = Math.floor(duration / 60);

            // Вычисляем целое количество секунд
            const seconds = Math.floor(duration % 60);

            // Конвертируем минуты в строку
            const minutesStr = minutes < 10 ? "0" + minutes : minutes;

            // Конвертируем секунды в строку
            const secondsStr = seconds < 10 ? "0" + seconds : seconds;

            // Выводим в элемент значение таймера
            displayElement.innerHTML = minutesStr + ":" + secondsStr;

            // Уменьшаем значение таймера 
            duration--;

            // Ставим таймаут на 1 секунду
        }, 1000);

        // Запускаем таймаут на отключение таймера
        setTimeout(() => {

            // Очищаем запущенный ранее интервал
            clearInterval(timerId);

            // Ставим таймаут на заданную длительность таймера в секундах
        }, duration * 1000);
    }
}