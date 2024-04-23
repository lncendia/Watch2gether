import * as yup from 'yup';

const roomSchema = yup.object().shape({
    id: yup.string().required(),
    viewersCount: yup.number().required(),
    isPrivate: yup.boolean().required(),
});

export const filmRoomShortSchema = roomSchema.shape({
    filmId: yup.string().uuid().required(),
    title: yup.string().required(),
    posterUrl: yup.string().required(),
    year: yup.number().required(),
    ratingKp: yup.number().min(0).max(10).nullable(),
    ratingImdb: yup.number().min(0).max(10).nullable(),
    userRating: yup.number().min(0).max(10).required(),
    description: yup.string().required(),
    isSerial: yup.boolean().required(),
});

export const filmRoomSchema = filmRoomShortSchema.shape({
    userRatingsCount: yup.number().required(),
    userScore: yup.number().min(0).max(10).nullable(),
    isCodeNeeded: yup.boolean().required(),
});

export const filmRoomsSchema = yup.object().shape({
    rooms: yup.array().of(filmRoomShortSchema).required(),
    countPages: yup.number().required()
});

export const youtubeRoomShortSchema = roomSchema.shape({
    videoAccess: yup.boolean().required(),
});

export const youtubeRoomSchema = youtubeRoomShortSchema.shape({
    isCodeNeeded: yup.boolean().required(),
});

export const youtubeRoomsSchema = yup.object().shape({
    rooms: yup.array().of(youtubeRoomShortSchema).required(),
    countPages: yup.number().required()
});



export const roomServerSchema = yup.object().shape({
    id: yup.string().required(),
    url: yup.string().required(),
    code: yup.string().min(5).max(5).nullable()
});