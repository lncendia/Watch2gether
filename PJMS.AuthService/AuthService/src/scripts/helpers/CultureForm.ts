/**
 * Класс функционала формы языка текущей страницы
 */
export class CultureForm {

    /**
     * Метод запускает функционал формы языка текущей страницы
     */
    startCultureForm() {

        // Получаем элемент формы
        const form: HTMLFormElement = document.querySelector('.form-culture') as HTMLFormElement;
        
        // добавляем обработчик изменения списка
        form.querySelector('select').addEventListener('change', () => {
            
            // Отправляем форму
            form.submit();
        });
    }
}