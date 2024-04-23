import '../styles/index.scss';
import 'bootstrap'
import 'jquery-validation-unobtrusive'


import {ResetTwoFactor} from "./pages/ResetTwoFactor";
import {LoginTwoStep} from "./pages/LoginTwoStep";
import {VerifySetupTwoFactor} from "./pages/VerifySetupTwoFactor";
import {Login} from "./pages/Login";
import {NewPassword} from "./pages/NewPassword";
import {RecoverPassword} from "./pages/RecoverPassword";
import {Registration} from "./pages/Registration";
import {Settings} from "./pages/Settings";
import {SetupTwoFactor} from "./pages/SetupTwoFactor";
import {CultureForm} from "./helpers/CultureForm";
import {ThemeToggler} from "./helpers/ThemeToggler";


//по загрузке страницы
window.addEventListener("load", () => {

    //получаем текущий URL
    const currentUrl = new URL(document.location.href);

    //получаем путь из URL
    const pathname = currentUrl.pathname.toLowerCase();

    //разбиваем пути URL на части
    const partsPath = pathname.split("/");

    // Создаем класс формы языка текущей страницы
    const cultureForm = new CultureForm();

    // Запускаем функционал формы языка текущей страницы
    cultureForm.startCultureForm();

    // Создаем класс переключателя темы
    const themeToggler = new ThemeToggler()

    // Запускаем функционал переключателя темы
    themeToggler.startThemeToggler()

    // Смотрим имя контроллера
    switch (partsPath[1]) {

        // Контроллер Account
        case "account": {

            // Смотрим имя метода
            switch (partsPath[2]) {

                // Метод Login
                case "login": {

                    // Создаем класс авторизации
                    const login = new Login();

                    // Запускаем функционал страницы авторизации
                    login.startAccount();

                    // Прерываем
                    break;
                }
                // Метод NewPassword
                case "newpassword": {

                    // Создаем класс страницы установки нового пароля
                    const password = new NewPassword();

                    // Запускаем функционал страницы установки пароля
                    password.startNewPassword()

                    // Прерываем
                    break;
                }
                // Метод RecoverPassword
                case "recoverpassword": {

                    // Создаем класс страницы установки нового пароля
                    const password = new RecoverPassword();

                    // Запускаем функционал страницы установки пароля
                    password.startRecoverPassword()

                    // Прерываем
                    break;
                }
            }
            // Прерываем
            break;
        }

        // Контроллер Registration
        case "registration": {

            // Смотрим имя метода
            switch (partsPath[2]) {

                // Метод Registration
                case "registration": {

                    // Создаем класс регистрации

                    const registration = new Registration();
                    // Запускаем функционал страницы регистрации

                    registration.startRegistration();

                    // Прерываем
                    break;
                }
            }

            // Прерываем
            break;
        }

        // Контроллер Settings
        case "settings": {

            // Смотрим имя метода
            switch (partsPath[2]) {

                // Метод Index
                default: {

                    // Создаем класс настроек
                    const settings = new Settings();

                    // Запускаем функционал страницы настроек
                    settings.startSettings();

                    // Прерываем
                    break;
                }
            }

            // Прерываем
            break;
        }

        // Контроллер TwoFactor
        case "twofactor": {

            // Смотрим имя метода
            switch (partsPath[2]) {

                // Метод Setup
                case "setup": {

                    // Создаем класс установки 2фа
                    const setupTwoFactor = new SetupTwoFactor()

                    // Запускаем функционал страницы установки 2фа
                    setupTwoFactor.startSetupTwoFactor()

                    // Прерываем
                    break;
                }

                // Метод LoginTwoStep
                case "logintwostep": {

                    // Создаем класс прохождения 2фа
                    const loginTwoStep = new LoginTwoStep()

                    // Запускаем функционал страницы прохождения 2фа
                    loginTwoStep.startLoginTwoStep();

                    // Прерываем
                    break;
                }

                // Метод Reset
                case "reset": {

                    // Создаем класс сброса 2фа
                    const resetTwoFactor = new ResetTwoFactor()

                    // Запускаем функционал страницы сброса 2фа
                    resetTwoFactor.startResetTwoStep();

                    // Прерываем
                    break;
                }

                // Метод VerifySetupTwoFactor
                case "verifysetup": {

                    // Создаем класс кодов восстановления
                    const verifySetup = new VerifySetupTwoFactor();

                    // Запускаем функционал страницы c кодами восстановления
                    verifySetup.startVerifySetup();

                    // Прерываем
                    break;
                }
            }

            // Прерываем
            break;
        }
    }
});