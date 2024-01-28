(() => {
    //по загрузке страницы
    window.addEventListener("load", () => {
        
        //получаем текущий URL
        const currentUrl = new URL(document.location.href);

        //получаем путь из URL
        const pathname = currentUrl.pathname.toLowerCase();

        //разбиваем пути URL на части
        const partsPath = pathname.split("/");

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
                    // Метод NewPassword
                    case "resetpassword": {

                        // Создаем класс страницы установки нового пароля
                        const password = new ResetPassword();

                        // Запускаем функционал страницы установки пароля
                        password.startResetPassword()

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
        }
    });
})();