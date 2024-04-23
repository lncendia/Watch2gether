import QRCode from "qrcode";
import {InputWrapper} from "../helpers/InputWrapper";

/**
* Класс функционала страницы установки 2фа
*/
export class SetupTwoFactor {
    
    /**
     * Метод запускает функционал страницы установки 2фа
     */
    startSetupTwoFactor() {

        // получаем все поля ввода с классом .wrap-input input
        new InputWrapper('.wrap-input input');
        
        // получаем элемент div с классом .auth-key
        const authKey = document.querySelector('.auth-key');

        // добавляем обработчик события клика
        authKey.addEventListener('click', ev => this.copyCode((ev.currentTarget as HTMLDivElement)));
        
        const qrCanvas = document.getElementById('qrCode');
        
        const qrData = qrCanvas.getAttribute("qr-data");
        
        QRCode.toCanvas(qrCanvas, qrData).then()
    }
    
    /**
     * Метод для копирования текста из элемента
     */
    copyCode(element: HTMLDivElement) {
        
        // Получаем текст для копирования, удаляя пробелы
        const textToCopy = element.textContent?.trim();
        
        // Копируем в буфер обмена
        if (textToCopy) {
            navigator.clipboard.writeText(textToCopy);
        }
    }
}