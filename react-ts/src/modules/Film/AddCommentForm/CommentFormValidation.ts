import * as yup from 'yup';

const commentFormValidationSchema = yup.object().shape({
    comment: yup
        .string()
        .required('Поле не должно быть пустым')
        .max(1000, 'Не больше 1000 символов')
});

export default commentFormValidationSchema;