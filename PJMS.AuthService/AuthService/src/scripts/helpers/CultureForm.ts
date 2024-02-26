/**
 * Класс функционала формы языка текущей страницы
 */
export class CultureForm {

    /**
     * Метод запускает функционал формы языка текущей страницы
     */
    startCultureForm() {

        // Получаем элемент формы
        const form: HTMLFormElement = document.querySelector('#culture-form') as HTMLFormElement;
        
        // добавляем обработчик изменения списка
        document.querySelector('#culture-form-select').addEventListener('change', () => {
            
            // Отправляем форму
            form.submit();
        });
    }
}