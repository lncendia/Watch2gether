/**
 * Класс добавляет функциональность для просмотра введенного пароля
 */
export class PasswordHide {

    /**
     * Иконка переключателя
     */
    private show: HTMLElement

    /**
     * Иконка переключателя
     */
    private hide: HTMLElement

    /**
     * Элемент ввода
     */
    private input: Element

    /**
     * Конструктор
     */
    constructor(selector: string) {

        // Объявляем переменную showPass и устанавливаем начальное значение false
        let showPass: boolean = false;

        // Находим элемент на странице с помощью селектора и сохраняем его в переменной container
        let container = document.querySelector(selector);

        // Находим следующий элемент после container и сохраняем его в свойстве input
        this.input = container.nextElementSibling;

        // Находим элемент с атрибутом password="show" внутри container и сохраняем его в свойстве show
        this.show = container.querySelector('[password="show"]');

        // Находим элемент с атрибутом password="hide" внутри container и сохраняем его в свойстве hide
        this.hide = container.querySelector('[password="hide"]');

        // Добавляем обработчик события 'click' на элемент
        container.addEventListener('click', () => {

            // Вызываем метод togglePassword с параметром showPass
            this.togglePassword(showPass);

            // Меняем его значение на противоположное
            showPass = !showPass;
        });

    }
    /**
     * Метод переключает видимость пароля
     */
    togglePassword(showPass: boolean) {
        // Проверяем, если showPass равно false
        if (!showPass) {
            
            // Если showPass равно false, скрываем элемент this.show и показываем элемент this.hide
            this.show.style.display = "none";
            this.hide.style.display = "block";
            
            // Устанавливаем тип атрибута input на 'text'
            this.input.setAttribute('type', 'text');
        } else {
            
            // Если showPass не равно false (т.е. true), показываем элемент this.show и скрываем элемент this.hide
            this.show.style.display = "block";
            this.hide.style.display = "none";
            
            // Устанавливаем тип атрибута input на 'password'
            this.input.setAttribute('type', 'password');
        }
    }
}