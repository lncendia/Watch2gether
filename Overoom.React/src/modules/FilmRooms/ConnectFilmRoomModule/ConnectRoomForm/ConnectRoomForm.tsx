import Form from "react-bootstrap/Form";
import {Button} from "react-bootstrap";
import {useFormik} from "formik";
import {connectFilmRoomFormSchema} from "./ConnectRoomFormValidation.ts";

interface ConnectRoomFormProps {
    warning?: string,
    code?: string,
    callback: (code: string) => void,
    onChange: () => void
}

const ConnectRoomForm = (props: ConnectRoomFormProps) => {
    const formik = useFormik({
        initialValues: {
            code: props.code ?? '',
        },
        validationSchema: connectFilmRoomFormSchema,
        onSubmit: (values: { code: string }) => {
            props.callback(values.code);
        },
    });

    return (
        <Form onSubmit={formik.handleSubmit}>
            <Form.Label>Код</Form.Label>

            <Form.Control
                name="code"
                value={formik.values.code}
                onChange={(e) => {
                    formik.handleChange(e);
                    props.onChange();
                }}
                isInvalid={!!formik.touched.code && (!!formik.errors.code || !!props.warning)}
            />

            <Form.Control.Feedback type="invalid">
                {formik.errors.code == null ? props.warning : formik.errors.code}
            </Form.Control.Feedback>

            <Button type="submit" variant="outline-danger" className="mt-5 w-100">
                Подключится
            </Button>
        </Form>
    );
};

export default ConnectRoomForm;