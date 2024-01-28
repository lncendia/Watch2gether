import React, {useState} from 'react';
import Form from "react-bootstrap/Form";
import {Button} from "react-bootstrap";

const AddCommentForm = ({callback}: { callback: (text: string) => void }) => {

    const [comment, addComment] = useState('')
    const [validated, setValidated] = useState(false);

    function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault()
        if (!(event.target as HTMLFormElement).checkValidity()) {
            setValidated(true);
        }
        else {
            setValidated(true);
            callback(comment)
        }
    }

    return (
        <Form noValidate validated={validated} onSubmit={handleSubmit}>
            <Form.Label asp-for="Text"/>
            <Form.Control required value={comment} onChange={event => addComment(event.target.value)} as="textarea" rows={2}/>
            <Form.Control.Feedback type="invalid">Please choose a username.</Form.Control.Feedback>
            <Button type="submit" variant="outline-danger" className="mt-2">Добавить</Button>
        </Form>
    );
};

export default AddCommentForm;