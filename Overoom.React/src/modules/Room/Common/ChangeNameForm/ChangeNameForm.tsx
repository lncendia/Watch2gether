import {Formik} from "formik";
import Form from "react-bootstrap/Form";
import {Button} from "react-bootstrap";
import {changeNameFormSchema} from "./ChangeNameFormValidation.ts";

const ChangeNameForm = ({callback, username = '', self}: {
    self: boolean,
    username?: string,
    callback: (username: string) => void
}) => {

    const handleSubmit = (values: { username: string; }) => {
        callback(values.username)
    };

    return (
        <Formik validationSchema={changeNameFormSchema} onSubmit={handleSubmit}
                initialValues={{username: username}}>
            {({handleSubmit, handleChange, values, touched, errors}) => (
                <Form onSubmit={handleSubmit}>
                    <Form.Label>Введите новое имя для {self ? "себя" : username}</Form.Label>
                    <Form.Control name="username" value={values.username} onChange={handleChange}
                                  isInvalid={touched.username && !!errors.username}/>
                    <Form.Control.Feedback type="invalid">{errors.username}</Form.Control.Feedback>

                    <Button type="submit" variant="outline-danger" className="mt-3 w-100">Изменить</Button>
                </Form>
            )}
        </Formik>
    );
};

export default ChangeNameForm;