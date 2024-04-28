import * as yup from 'yup';

const createFilmRoomFormSchema = yup.object().shape({
    open: yup.boolean().required('Укажите тип комнаты'),
    cdn: yup.string().required('Вы должны выбрать CDN'),
});

export default createFilmRoomFormSchema