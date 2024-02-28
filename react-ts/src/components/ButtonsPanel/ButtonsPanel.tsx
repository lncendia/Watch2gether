import {Button} from "@mui/material";
import {ButtonContent} from "./ButtonContent.ts";
import Box from "@mui/material/Box";

// Компонент панели с кнопками
const ButtonsPanel = ({buttons}: { buttons: ButtonContent[] }) => {

    // Создаем массив кнопок на основе ButtonContent переданный через props
    return (
        <div className="sign-buttons">
            <Box sx={{'& button': {m: 1}}}>
                {buttons.map((btn, index) => (
                    <Button
                        key={index}
                        variant="contained"
                        onClick={() => btn.action()}
                        size="small"
                    >{btn.title}</Button>
                ))}
            </Box>
        </div>
    )
}

export default ButtonsPanel;