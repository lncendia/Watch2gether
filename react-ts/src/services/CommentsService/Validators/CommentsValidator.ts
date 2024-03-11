import * as yup from 'yup';

const commentSchema = yup.object().shape({
    id: yup.string().required(),
    username: yup.string().required(),
    text: yup.string().required(),
    avatarUrl: yup.string().nullable(),
    createdAt: yup.date().required(),
    isUserComment: yup.boolean().required(),
});

const commentsSchema = yup.object().shape({
    comments: yup.array().of(commentSchema).required(),
    countPages: yup.number().integer().min(0).required(),
});

export { commentSchema, commentsSchema };
