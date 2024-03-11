import * as yup from 'yup';

const actorSchema = yup.object().shape({
    name: yup.string().required(),
    description: yup.string().nullable(),
});

const cdnSchema = yup.object().shape({
    cdn: yup.string().required(),
    quality: yup.string().required(),
    url: yup.string().required()
});

const filmSchema = yup.object().shape({
    id: yup.string().uuid().required(),
    description: yup.string().required(),
    isSerial: yup.boolean().required(),
    year: yup.number().positive().required(),
    title: yup.string().required(),
    posterUrl: yup.string().required(),
    ratingKp: yup.number().min(0).max(10).nullable(),
    ratingImdb: yup.number().min(0).max(10).nullable(),
    userRating: yup.number().min(0).max(10).required(),
    userRatingsCount: yup.number().required(),
    userScore: yup.number().min(0).max(10).nullable(),
    inWatchlist: yup.boolean().nullable(),
    cdnList: yup.array().of(cdnSchema).required(),
    countSeasons: yup.number().min(0).nullable(),
    countEpisodes: yup.number().min(0).nullable(),
    genres: yup.array().of(yup.string()).required(),
    countries: yup.array().of(yup.string()).required(),
    directors: yup.array().of(yup.string()).required(),
    screenWriters: yup.array().of(yup.string()).required(),
    actors: yup.array().of(actorSchema).required(),
});

export {filmSchema};