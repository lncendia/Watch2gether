export class ThemeToggler {

    light: HTMLElement
    dark: HTMLElement

    startThemeToggler() {

        let theme = window.localStorage.getItem("theme");

        if (theme == "null") theme = "light"

        const toggler = document.querySelector(".theme-toggler")
        this.light = toggler.querySelector("#light");
        this.dark = toggler.querySelector("#dark");

        toggler.addEventListener("click", () => this.toggleTheme())

        this.setTheme(theme);
    }

    toggleTheme() {
        let theme = window.localStorage.getItem("theme");
        if (theme === "light") this.setTheme("dark")
        else if (theme === "dark") this.setTheme("light")
    }

    setTheme(theme: string) {
        document.querySelector('body').setAttribute("data-bs-theme", theme);
        window.localStorage.setItem("theme", theme);
        if (theme === "light") {
            this.light.style.display = "none"
            this.dark.style.display = "block"
        } else if (theme === "dark") {
            this.light.style.display = "block"
            this.dark.style.display = "none"
        }
    }
}