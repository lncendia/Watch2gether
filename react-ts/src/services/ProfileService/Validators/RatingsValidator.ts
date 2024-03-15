import * as yup from 'yup';

const ratingSchema = yup.object().shape({
    filmId: yup.string().uuid().required(),
    title: yup.string().required(),
    year: yup.number().positive().required(),
    ratingKp: yup.number().min(0).max(10).nullable(),
    ratingImdb: yup.number().min(0).max(10).nullable(),
    posterUrl: yup.string().required(),
    score: yup.number().min(0).max(10).required(),
});

const ratingsSchema = yup.object().shape({
    ratings: yup.array().of(ratingSchema).required(),
    countPages: yup.number().required(),
});


export {ratingsSchema}