import {InputWrapper} from "../helpers/InputWrapper";

/**
 * Класс функционала страницы восстановления пароля
 */
export class RecoverPassword {
    
    /**
     * Метод запускает функционал страницы восстановления пароля
     */
    startRecoverPassword() {
        
        // получаем все поля ввода с классом .wrap-input input
        new InputWrapper('.wrap-input input');
    }
}