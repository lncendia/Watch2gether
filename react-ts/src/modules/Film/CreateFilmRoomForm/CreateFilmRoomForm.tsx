import {Formik} from "formik";
import Form from "react-bootstrap/Form";
import {Button} from "react-bootstrap";
import {Cdn} from "../../../services/FilmsService/Models/Film.ts";
import Offcanvas from "../../../components/Comments/Offcanvas/Offcanvas.tsx";
import createFilmRoomFormSchema from "./CreateFilmRoomFormValidation.ts";

interface CreateFilmRoomFormProps {
    open: boolean,
    onClose: () => void,
    cdnList: Cdn[],
    callback: (cdn: string, open: boolean) => void
}

const CreateFilmRoomForm = ({open, cdnList, onClose, callback}: CreateFilmRoomFormProps) => {

    const handleSubmit = (values: { cdn: string; open: boolean }) => {
        callback(values.cdn, values.open)
    };

    return (
        <Offcanvas title="Создание комнаты с фильмом" show={open} onClose={onClose}>
            <Formik validationSchema={createFilmRoomFormSchema} onSubmit={handleSubmit}
                    initialValues={{cdn: '', open: false}}>
                {({handleSubmit, handleChange, values, touched, errors}) => (
                    <Form onSubmit={handleSubmit}>

                        <Form.Select className="mt-2" name="cdn" value={values.cdn} onChange={handleChange}
                                     isInvalid={touched.cdn && !!errors.cdn}>
                            <option>Выберите CDN</option>
                            {cdnList.map(c => <option key={c.cdn} value={c.cdn}>{c.cdn} ({c.quality})</option>)}
                        </Form.Select>
                        <Form.Control.Feedback type="invalid">{errors.cdn}</Form.Control.Feedback>

                        <Form.Check
                            name="open"
                            label="Публичная комната"
                            onChange={handleChange}
                            isInvalid={touched.open && !!errors.open}
                            feedback={errors.open}
                            feedbackType="invalid"
                            id="open"
                            className="mt-3"
                        />

                        <Button type="submit" variant="outline-danger" className="mt-5 w-100">Создать</Button>
                    </Form>
                )}
            </Formik>
        </Offcanvas>
    );
};

export default CreateFilmRoomForm;