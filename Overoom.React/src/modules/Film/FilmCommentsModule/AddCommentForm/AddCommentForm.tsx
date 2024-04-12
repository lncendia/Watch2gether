import Form from "react-bootstrap/Form";
import {Button} from "react-bootstrap";
import {useFormik} from "formik";
import addCommentFormSchema from "./AddCommentFormValidation.ts";

const AddCommentForm = ({callback}: { callback: (text: string) => void }) => {
    const formik = useFormik({
        initialValues: {
            comment: '',
        },
        validationSchema: addCommentFormSchema,
        onSubmit: (values) => {
            callback(values.comment);
        },
    });

    return (
        <Form onSubmit={formik.handleSubmit}>
            <Form.Label>Оставьте комментарий</Form.Label>
            <Form.Control
                name="comment"
                value={formik.values.comment}
                onChange={formik.handleChange}
                isInvalid={formik.touched.comment && !!formik.errors.comment}
                as="textarea"
                rows={3}
            />
            <Form.Control.Feedback type="invalid">{formik.errors.comment}</Form.Control.Feedback>
            <Button type="submit" variant="outline-danger" className="mt-2">
                Добавить
            </Button>
        </Form>
    );
};

export default AddCommentForm;