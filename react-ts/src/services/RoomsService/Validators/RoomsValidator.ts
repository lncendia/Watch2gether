import * as yup from 'yup';

const roomSchema = yup.object().shape({
    id: yup.string().required(),
    viewersCount: yup.number().required(),
    isCodeNeeded: yup.boolean().required(),
});

export const filmRoomSchema = roomSchema.shape({
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

export const youtubeRoomSchema = roomSchema.shape({
    videoAccess: yup.boolean().required(),
});

export const filmRoomsSchema = yup.object().shape({
    rooms: yup.array().of(filmRoomSchema).required(),
    countPages: yup.number().required()
});

export const youtubeRoomsSchema = yup.object().shape({
    rooms: yup.array().of(youtubeRoomSchema).required(),
    countPages: yup.number().required()
});