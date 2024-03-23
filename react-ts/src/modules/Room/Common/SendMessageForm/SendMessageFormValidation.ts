import * as yup from 'yup';

const sendMessageFormSchema = yup.object().shape({
    text: yup
        .string()
        .required('Поле не должно быть пустым')
        .max(1000, 'Не больше 1000 символов')
});

export default sendMessageFormSchema;