import * as yup from "yup";

// Валидация ответа сервера
export const filmShortSchema = yup.object({
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
export const filmsSchema = yup.object({
    films: yup.array().of(filmShortSchema).required(),
    countPages: yup.number().required(),
});

const actorSchema = yup.object().shape({
    name: yup.string().required(),
    description: yup.string().nullable(),
});

const cdnSchema = yup.object().shape({
    cdn: yup.string().required(),
    quality: yup.string().required(),
    url: yup.string().required()
});

export const filmSchema = filmShortSchema.shape({
    userRatingsCount: yup.number().required(),
    userScore: yup.number().min(0).max(10).nullable(),
    inWatchlist: yup.boolean().nullable(),
    cdnList: yup.array().of(cdnSchema).required(),
    countries: yup.array().of(yup.string()).required(),
    directors: yup.array().of(yup.string()).required(),
    screenWriters: yup.array().of(yup.string()).required(),
    actors: yup.array().of(actorSchema).required(),
});
