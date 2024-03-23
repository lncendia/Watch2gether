import * as yup from 'yup';

const roomServerSchema = yup.object().shape({
    id: yup.string().required(),
    url: yup.string().required(),
    code: yup.string().min(5).max(5).nullable()
});

export {roomServerSchema}