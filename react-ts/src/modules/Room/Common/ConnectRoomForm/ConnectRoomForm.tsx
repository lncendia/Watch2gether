import {Formik} from "formik";
import Form from "react-bootstrap/Form";
import {Button} from "react-bootstrap";
import {connectFilmRoomFormSchema} from "./ConnectRoomFormValidation.ts";

interface ConnectRoomFormProps {
    warning: string,
    code?: string,
    callback: (code: string) => void,
    onChange: () => void
}

const ConnectRoomForm = (props: ConnectRoomFormProps) => {

    const handleSubmit = (values: { code: string; }) => {
        props.callback(values.code)
    };

    return (
        <Formik validationSchema={connectFilmRoomFormSchema} onSubmit={handleSubmit}
                initialValues={{code: props.code}}>
            {({handleSubmit, handleChange, values, touched, errors}) => (
                <Form onSubmit={handleSubmit}>
                    <Form.Label>Код</Form.Label>

                    <Form.Control name="code" value={values.code} onChange={(e) => {
                        handleChange(e)
                        props.onChange()
                    }} isInvalid={touched.code && (!!errors.code || props.warning)}/>

                    <Form.Control.Feedback type="invalid">
                        {errors.code == null ? props.warning : errors.code}
                    </Form.Control.Feedback>

                    <Button type="submit" variant="outline-danger" className="mt-5 w-100">Подключится</Button>
                </Form>
            )}
        </Formik>
    );
};

export default ConnectRoomForm;