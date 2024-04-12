import {useFormik} from "formik";
import Form from "react-bootstrap/Form";
import {Button} from "react-bootstrap";
import createFilmRoomFormSchema from "./CreateFilmRoomFormValidation.ts";

interface CreateFilmRoomFormProps {
    cdnList: [string, string][],
    callback: (cdn: string, open: boolean) => void
}

const CreateFilmRoomForm = ({cdnList, callback}: CreateFilmRoomFormProps) => {

    const formik = useFormik({
        initialValues: {
            cdn: '',
            open: false,
        },
        validationSchema: createFilmRoomFormSchema,
        onSubmit: (values) => {
            callback(values.cdn, values.open);
        },
    });

    return (
        <Form onSubmit={formik.handleSubmit}>
            <Form.Select
                className="mt-2"
                name="cdn"
                value={formik.values.cdn}
                onChange={formik.handleChange}
                isInvalid={formik.touched.cdn && !!formik.errors.cdn}
            >
                <option>Выберите CDN</option>
                {cdnList.map((cdn) => (
                    <option key={cdn[0]} value={cdn[0]}>
                        {cdn[0]} ({cdn[1]})
                    </option>
                ))}
            </Form.Select>
            <Form.Control.Feedback type="invalid">{formik.errors.cdn}</Form.Control.Feedback>

            <Form.Check
                type="checkbox"
                name="open"
                label="Публичная комната"
                onChange={formik.handleChange}
                id="open"
                className="mt-3"
            />

            <Button type="submit" variant="outline-danger" className="mt-5 w-100">
                Создать
            </Button>
        </Form>
    );
};

export default CreateFilmRoomForm;