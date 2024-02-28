// Тип ссылки на функцию
type Action = (arg?: string) => void;

// Интерфейс наполнения кнопки
export interface ButtonContent {
    
    // Надпись на кнопке
    title: string
    
    // Действие кнопки по нажатию
    action: Action
} 