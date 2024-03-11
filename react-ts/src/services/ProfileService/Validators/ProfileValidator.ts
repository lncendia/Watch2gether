import * as yup from 'yup';

const userFilmSchema = yup.object().shape({
    filmId: yup.string().required(),
    title: yup.string().required(),
    year: yup.number().positive().required(),
    ratingKp: yup.number().min(0).max(10).nullable(),
    ratingImdb: yup.number().min(0).max(10).nullable(),
    posterUrl: yup.string().required(),
});

const allowsSchema = yup.object().shape({
    beep: yup.boolean().required(),
    scream: yup.boolean().required(),
    change: yup.boolean().required(),
});

const profileSchema = yup.object().shape({
    allows: allowsSchema.required(),
    watchlist: yup.array().of(userFilmSchema).required(),
    history: yup.array().of(userFilmSchema).required(),
    genres: yup.array().of(yup.string()).required(),
});

export {profileSchema}