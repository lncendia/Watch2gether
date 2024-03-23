import Form from "react-bootstrap/Form";
import {Button} from "react-bootstrap";
import {Formik} from "formik";
import addCommentFormSchema from "./AddCommentFormValidation.ts";

const AddCommentForm = ({callback}: { callback: (text: string) => void }) => {
    const handleSubmit = (values: { comment: string; }) => {
        callback(values.comment)
    };

    return (
        <Formik validationSchema={addCommentFormSchema} onSubmit={handleSubmit}
                initialValues={{comment: ''}}>
            {({handleSubmit, handleChange, values, touched, errors}) => (
                <Form onSubmit={handleSubmit}>
                    <Form.Label>Оставьте комментарий</Form.Label>
                    <Form.Control name="comment" value={values.comment} onChange={handleChange}
                                  isInvalid={touched.comment && !!errors.comment} as="textarea" rows={3}/>
                    <Form.Control.Feedback type="invalid">{errors.comment}</Form.Control.Feedback>
                    <Button type="submit" variant="outline-danger" className="mt-2">Добавить</Button>
                </Form>
            )}
        </Formik>
    );
};

export default AddCommentForm;