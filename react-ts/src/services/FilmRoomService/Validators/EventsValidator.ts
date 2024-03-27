import * as yup from 'yup';

export const messageSchema = yup.object().shape({
    id: yup.string().uuid().required(),
    userId: yup.string().uuid().required(),
    createdAt: yup.date().required(),
    text: yup.string().required(),
});

export const messagesSchema = yup.object().shape({
    messages: yup.array().of(messageSchema).required(),
    count: yup.number().required(),
});

export const actionSchema = yup.object().shape({
    initiator: yup.string().uuid().required(),
    target: yup.string().uuid().required()
});

export const changeNameSchema = actionSchema.shape({
    name: yup.string().required(),
});