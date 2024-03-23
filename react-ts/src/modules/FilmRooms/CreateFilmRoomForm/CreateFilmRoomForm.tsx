import {Formik} from "formik";
import Form from "react-bootstrap/Form";
import {Button} from "react-bootstrap";
import createFilmRoomFormSchema from "./CreateFilmRoomFormValidation.ts";

interface CreateFilmRoomFormProps {
    cdnList: [string, string][],
    callback: (cdn: string, open: boolean) => void
}

const CreateFilmRoomForm = ({cdnList, callback}: CreateFilmRoomFormProps) => {

    const handleSubmit = (values: { cdn: string; open: boolean }) => {
        callback(values.cdn, values.open)
    };

    return (
        <Formik validationSchema={createFilmRoomFormSchema} onSubmit={handleSubmit}
                initialValues={{cdn: '', open: false}}>
            {({handleSubmit, handleChange, values, touched, errors}) => (
                <Form onSubmit={handleSubmit}>

                    <Form.Select className="mt-2" name="cdn" value={values.cdn} onChange={handleChange}
                                 isInvalid={touched.cdn && !!errors.cdn}>
                        <option>Выберите CDN</option>
                        {cdnList.map(cdn => <option key={cdn[0]} value={cdn[0]}>{cdn[0]} ({cdn[1]})</option>)}
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
    );
};

export default CreateFilmRoomForm;