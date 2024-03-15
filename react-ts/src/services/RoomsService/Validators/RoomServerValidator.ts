import * as yup from 'yup';

const roomServerSchema = yup.object().shape({
    id: yup.string().required(),
    url: yup.string().required(),
});

export {roomServerSchema}