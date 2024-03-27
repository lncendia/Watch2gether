import {Formik} from "formik";
import Form from "react-bootstrap/Form";
import {Button} from "react-bootstrap";
import {connectFilmRoomFormSchema} from "./ConnectRoomFormValidation.ts";

const ConnectRoomForm = ({callback, code = ''}: { code?: string, callback: (code: string) => void }) => {

    const handleSubmit = (values: { code: string; }) => {
        callback(values.code)
    };

    return (
        <Formik validationSchema={connectFilmRoomFormSchema} onSubmit={handleSubmit}
                initialValues={{code: code}}>
            {({handleSubmit, handleChange, values, touched, errors}) => (
                <Form onSubmit={handleSubmit}>
                    <Form.Label>Код</Form.Label>
                    <Form.Control name="code" value={values.code} onChange={handleChange}
                                  isInvalid={touched.code && !!errors.code}/>
                    <Form.Control.Feedback type="invalid">{errors.code}</Form.Control.Feedback>

                    <Button type="submit" variant="outline-danger" className="mt-5 w-100">Подключится</Button>
                </Form>
            )}
        </Formik>
    );
};

export default ConnectRoomForm;