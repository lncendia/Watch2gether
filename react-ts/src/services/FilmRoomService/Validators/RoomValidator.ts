import * as yup from 'yup';

const allowsSchema = yup.object().shape({
    beep: yup.boolean().required(),
    scream: yup.boolean().required(),
    change: yup.boolean().required(),
});

const filmViewerSchema = yup.object().shape({
    season: yup.number().nullable(),
    series: yup.number().nullable(),
    id: yup.string().uuid().required(),
    username: yup.string().required(),
    photoUrl: yup.string().nullable(),
    pause: yup.boolean().required(),
    fullScreen: yup.boolean().required(),
    online: yup.boolean().required(),
    second: yup.number().required(),
    allows: allowsSchema.required(),
});

const filmRoomSchema = yup.object().shape({
    title: yup.string().required(),
    cdnName: yup.string().required(),
    cdnUrl: yup.string().url().required(),
    isSerial: yup.boolean().required(),
    id: yup.string().uuid().required(),
    ownerId: yup.string().uuid().required(),
    viewers: yup.array().of(filmViewerSchema).required(),
});

export default filmRoomSchema