/**
 * Класс добавляет функциональность для смены темы
 */
export class ThemeToggler {

    /**
     * Переключатель на светлую тему
     */
    private light: HTMLElement

    /**
     * Переключатель на темную тему
     */
    private dark: HTMLElement

    /**
     * Метод запускает функционал переключения темы
     */
    startThemeToggler() {

        // Получаем текущую тему из локального хранилища и сохраняем в переменной theme
        let theme = window.localStorage.getItem("theme");

        // Проверяем, если текущая тема равна "null", устанавливаем значение "light" для переменной theme
        if (theme == "null") theme = "light";

        // Находим элемент с классом "theme-toggler" и сохраняем его в переменной toggler
        const toggler = document.querySelector(".theme-toggler");

        // Находим элемент с id "light" внутри элемента toggler и сохраняем его в свойство light
        this.light = toggler.querySelector("#light");

        // Находим элемент с id "dark" внутри элемента toggler и сохраняем его в свойство dark
        this.dark = toggler.querySelector("#dark");

        // Добавляем обработчик события "click" на элемент toggler, который вызывает метод toggleTheme
        toggler.addEventListener("click", () => this.toggleTheme());

        // Устанавливаем тему, используя значение переменной theme
        this.setTheme(theme);
    }

    /**
     * Метод переключает тему
     */
    toggleTheme() {
        // Получаем текущую тему из локального хранилища и сохраняем в переменной theme
        let theme = window.localStorage.getItem("theme");

        // Проверяем, если текущая тема "light", вызываем метод setTheme с аргументом "dark"
        if (theme === "light") this.setTheme("dark");

        // Иначе, если текущая тема "dark", вызываем метод setTheme с аргументом "light"
        else if (theme === "dark") this.setTheme("light");
    }

    /**
     * Метод устанавливает тему
     */
    setTheme(theme: string) {

        // Устанавливаем атрибут "data-bs-theme" со значением темы на теге <body>
        document.querySelector('body').setAttribute("data-bs-theme", theme);

        // Сохраняем текущую тему в локальном хранилище браузера
        window.localStorage.setItem("theme", theme);

        // Проверяем значение переменной theme и устанавливаем соответствующие стили для элементов this.light и this.dark
        if (theme === "light") {

            // Если тема "light", скрываем элемент this.light и показываем элемент this.dark
            this.light.style.display = "none";
            this.dark.style.display = "block";
        } else if (theme === "dark") {

            // Если тема "dark", скрываем элемент this.dark и показываем элемент this.light
            this.light.style.display = "block";
            this.dark.style.display = "none";
        }
    }
}