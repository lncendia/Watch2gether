import ContentBlock from "../../../UI/ContentBlock/ContentBlock.tsx";
import {Button} from "react-bootstrap";

const ProfileSettingsModule = ({className}: { className?: string }) => {

    return (
        <ContentBlock className={className}>
            <Button href='https://localhost:10001/settings' target='_blank' className="w-100">Настройки профиля</Button>
        </ContentBlock>
    );
};

export default ProfileSettingsModule;