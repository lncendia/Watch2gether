import * as yup from "yup";

// Валидация ответа сервера
const playlistSchema = yup.object({
    id: yup.string().uuid().required(),
    name: yup.string().required(),
    description: yup.string().required(),
    genres: yup.array().of(yup.string()).required(),
    updated: yup.string().required()
});

// Валидация ответа сервера
const playlistsSchema = yup.object({
    playlists: yup.array().of(playlistSchema).required(),
    countPages: yup.number().required(),
});

export {playlistSchema, playlistsSchema}