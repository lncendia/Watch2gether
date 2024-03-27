import {Button} from "react-bootstrap";

const NotEnoughRights = ({goBack}: { goBack: () => void }) => {
    return (
        <>
            <h2>Для доступа к этой области требуется авторизация или недостаточно прав</h2>
            <div>
                <Button variant="contained" onClick={goBack}>Назад</Button>
            </div>
        </>
    );
};

export default NotEnoughRights;