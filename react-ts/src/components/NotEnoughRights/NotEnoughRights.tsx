import {Button} from "@mui/material";

// Компонент отображается если у пользователя недостаточно прав для посещения страницы
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