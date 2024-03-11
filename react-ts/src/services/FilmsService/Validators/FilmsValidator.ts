import * as yup from "yup";

// Валидация ответа сервера
const filmShortSchema = yup.object({
    id: yup.string().uuid().required(),
    title: yup.string().required(),
    posterUrl: yup.string().required(),
    year: yup.number().positive().required(),
    ratingKp: yup.number().min(0).max(10).nullable(),
    ratingImdb: yup.number().min(0).max(10).nullable(),
    userRating: yup.number().min(0).max(10).required(),
    description: yup.string().required(),
    isSerial: yup.boolean().required(),
    countSeasons: yup.number().integer().nullable(),
    countEpisodes: yup.number().integer().nullable(),
    genres: yup.array().of(yup.string()).required(),
});

// Валидация ответа сервера
const filmsSchema = yup.object({
    films: yup.array().of(filmShortSchema).required(),
    countPages: yup.number().required(),
});

export {filmsSchema, filmShortSchema}