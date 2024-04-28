import {Formik} from "formik";
import Form from "react-bootstrap/Form";
import sendMessageFormSchema from "./SendMessageFormValidation.ts";
import {useCallback} from "react";

interface SendMessageFormProps {
    className?: string,
    callback: (text: string) => void,
    type: () => void
}

const SendMessageForm = ({callback, className, type}: SendMessageFormProps) => {

    const handleSubmit = useCallback((values: { text: string; }) => {
        callback(values.text)
        values.text = ''
    }, [callback]);

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
                                      } else {
                                          type()
                                      }
                                  }}/>
                    <Form.Control.Feedback type="invalid">{errors.text}</Form.Control.Feedback>
                </Form>
            )}
        </Formik>
    );
};

export default SendMessageForm;