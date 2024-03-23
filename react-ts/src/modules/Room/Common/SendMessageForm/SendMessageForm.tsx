import {Formik} from "formik";
import Form from "react-bootstrap/Form";
import sendMessageFormSchema from "./SendMessageFormValidation.ts";

const SendMessageForm = ({callback, className}: { className?: string, callback: (text: string) => void }) => {
    const handleSubmit = (values: { text: string; }) => {
        callback(values.text)
        values.text = ''
    };

    return (
        <Formik validationSchema={sendMessageFormSchema} onSubmit={handleSubmit}
                initialValues={{text: ''}}>
            {({handleSubmit, handleChange, values, touched, errors}) => (
                <Form onSubmit={handleSubmit} className={className}>
                    <Form.Control placeholder="Отправьте сообщение" name="text" value={values.text}
                                  onChange={handleChange}
                                  isInvalid={touched.text && !!errors.text} as="textarea" rows={2}
                                  onKeyDown={(event) => {
                                      if (event.key === 'Enter' && !event.ctrlKey) {
                                          event.preventDefault();
                                          handleSubmit();
                                      }
                                  }}/>
                    <Form.Control.Feedback type="invalid">{errors.text}</Form.Control.Feedback>
                </Form>
            )}
        </Formik>
    );
};

export default SendMessageForm;