/**
 * Класс проверяет пароль на надежность
 */
export class PasswordStrengthValidator {

    /**
     * Блок элементов надежности пароля - ползунок прогресса и сообщения для пользователя
     */
    indicatorField: HTMLDivElement;
    
    /**
     * Блок с ошибками
     */
    errorsBlock: HTMLSpanElement;
    
    /**
     * Блок прогресс-бара
     */
    progress: HTMLDivElement;
    
    /**
     * Элемент отправляемой формы
     */
    form: HTMLFormElement;

    /**
     * Конструктор принимает блок с ползунком прогресса и сообщениями для пользователя и отпрвляемую форму
     */
    constructor(indicatorField: HTMLDivElement, form: HTMLFormElement) {
        
        // блок элементов надежности пароля
        this.indicatorField = indicatorField;
        
        // блок с ошибками
        this.errorsBlock = indicatorField.querySelector('.pass-valid-errors')
        
        // прогресс-бар
        this.progress = indicatorField.querySelector('.progress-bar')
        
        // отправляемая форма
        this.form = form;
    }

    /**
     * Метод оценивает надежность пароля и возвращает оценку в виде числа (от 0 до 5)
     */
    private getPasswordStrength(password: string): number {
        // урезание 2 и более идущих пробелов подряд
        password = password.replace(/\s{2,}/, ' ');

        // число, оценка надежности
        let strength: number = 0;

        // проверяем критерии надежности - наличие букв в двух регистрах, наличие цифр и спец символов
        [/\p{Ll}+/u, /\p{Lu}+/u, /\p{N}+/u, /[^\p{L}\p{N}]+/u].forEach(el => {

            // при совпадении увеличиваем оценку надежности на 1
            if (password.match(el)) strength++;
        });

        // длина также участвует в оценке надежности
        if (password.length >= 8) strength++;
        
        //Возвращаем оценку
        return strength;
    }

    /**
     * Метод проверяет пароль на надежность перед отправкой формы
     */
    validateFormPassword(passwordFieldId: string) {

        // получаем пароль из формы
        let password = this.form.querySelector<HTMLInputElement>(`#${passwordFieldId}`).value;

        // получаем оценку надежности пароля
        let passwordStrength = this.getPasswordStrength(password);

        // если надежность пароля максимальная
        if (passwordStrength === 5) {
            
            // отправляем форму
            this.form.submit();
        }
        // если пароль не является надежным - выводим сообщение на форму
        else {
            
            // выводим сообщение
            this.errorsBlock.innerHTML = this.indicatorField.querySelector('#invalid-pass').innerHTML;
            
            // выделяем его красным цветом
            this.errorsBlock.style.color = "var(--bs-red)";
        }
    }

    /**
     * Метод изменяет индикатор надежности в зависимости от пароля
     */
    checkPasswordStrength(password: string) {

        // Получаем элемент ползунка прогресса
        let progress: HTMLDivElement = this.indicatorField.querySelector('.progress-bar');
        
        // если длина пароля больше 3 символов, начинаем оценивать его надежность
        if (password.length > 0) {

            // делаем видимым блок с ползунком прогресса и комментарием 
            this.indicatorField.removeAttribute('hidden');

            // устанавливаем цвет сообщения о пароле по умолчанию 
            this.errorsBlock.style.color = "";

            // сообщение для пользователя о степени надежности
            let messageBlock: string;

            // стиль отвечает за цвет ползунка
            let style: string;

            // строка со старым стилем элемента
            let oldStyle: string = progress.classList.item(1);

            // получаем оценку надежности пароля
            let strength = this.getPasswordStrength(password);

            // при оценке от 0 до 2 - пароль слабый
            if (strength < 3) {
                messageBlock = '#week-pass';
                style = 'bg-danger'
            }
            // при оценке от 3 до 4 - пароль средний
            else if (strength >=3 && strength <= 4) {
                messageBlock = '#medium-pass';
                style = 'bg-warning';
            }
            // оценка 5 из 5 - пароль надежный
            else {
                messageBlock = '#strong-pass';
                style = 'bg-success';
            }

            // выбираем блок span и туда помещаем комментарий о надежности из одного из блоков
            this.errorsBlock.innerHTML = this.indicatorField.querySelector(messageBlock).innerHTML;

            // устанавливаем значение для ползунка прогресса надежности
            progress.setAttribute('aria-valuenow', String(strength * 20));

            // меняем цвет ползунка с помощью стиля
            progress.classList.replace(oldStyle, style);

            // также увеличиваем длину активной части ползунка (прогресса)
            progress.style.width = `${strength * 20}%`;
        }
        
        // Если длина пароля - 3 и менее символов
        else {
            
            // скрываем блок с показателем и комментарием
            this.indicatorField.setAttribute('hidden', 'hidden');
        }
    }
}