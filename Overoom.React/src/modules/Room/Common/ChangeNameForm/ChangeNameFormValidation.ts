import * as yup from 'yup';

export const changeNameFormSchema = yup.object().shape({
    username: yup.string().max(40, 'Не более 40 символов').required('Укажите имя'),
});