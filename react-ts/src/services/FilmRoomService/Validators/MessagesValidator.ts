import * as yup from 'yup';

const messageSchema = yup.object().shape({
    id: yup.string().uuid().required(),
    userId: yup.string().uuid().required(),
    createdAt: yup.date().required(),
    text: yup.string().required(),
});

const messagesSchema = yup.object().shape({
    messages: yup.array().of(messageSchema).required(),
    count: yup.number().required(),
});

export default messagesSchema