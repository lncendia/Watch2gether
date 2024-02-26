/**
 * Класс функционала страницы c кодами восстановления
 */
export class VerifySetupTwoFactor {

    /**
     * Метод запускает функционал страницы c кодами восстановления
     */
    startVerifySetup() {

        // Получаем кнопку с классом .copy-codes
        const copyButton = document.querySelector('.copy-codes');

        // Добавляем обработчик события клика
        copyButton.addEventListener('click', this.copyRecoveryCodes.bind(this));

    }

    /**
     * Метод для копирования текста из элементов, содержащие коды
     */
    copyRecoveryCodes() {

        // Получаем элементы с классом .codes
        const recoveryCodeElements = document.querySelectorAll('.codes');

        // Создаем пустую строку, которая будет содержать все коды восстановления
        let recoveryCodesToCopy = '';

        // Получаем код из каждого элемента и формируем строку
        recoveryCodeElements.forEach(element => {
            recoveryCodesToCopy += element.textContent?.trim() + '\n';
        });

        // Копируем в буфер
        navigator.clipboard.writeText(recoveryCodesToCopy).then();
    }
}