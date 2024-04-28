import * as yup from 'yup';

export const connectFilmRoomFormSchema = yup.object().shape({
    code: yup.string().min(5, 'Введите 5-ти значный код').max(5, 'Введите 5-ти значный код').required('Введите 5-ти значный код'),
});