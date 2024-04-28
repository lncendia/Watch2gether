import {useProfile} from "../../../contexts/ProfileContext/ProfileContext.tsx";
import Form from "react-bootstrap/Form";
import ContentBlock from "../../../UI/ContentBlock/ContentBlock.tsx";
import {useCallback} from "react";
import {useInjection} from "inversify-react";
import {IProfileService} from "../../../services/ProfileService/IProfileService.ts";

const UserAllowsModule = ({className}: { className?: string }) => {

    const {allows, changeAllows} = useProfile()

    const profileService = useInjection<IProfileService>('ProfileService');

    const onBeep = useCallback(() => {
        const newAllows = {...allows, beep: !allows.beep}
        profileService.changeAllows(newAllows).then(() => changeAllows(newAllows))
    }, [allows, profileService, changeAllows])

    const onScream = useCallback(() => {
        const newAllows = {...allows, scream: !allows.scream}
        profileService.changeAllows(newAllows).then(() => changeAllows(newAllows))
    }, [allows, profileService, changeAllows])

    const onChange = useCallback(() => {
        const newAllows = {...allows, change: !allows.change}
        profileService.changeAllows(newAllows).then(() => changeAllows(newAllows))
    }, [allows, profileService, changeAllows])

    return (
        <ContentBlock className={className}>
            <Form>
                <Form.Check
                    onChange={onBeep}
                    className="mb-3"
                    checked={allows.beep}
                    type="switch"
                    name="beep"
                    id="beep"
                    label="Звуковой сигнал"
                />
                <Form.Check
                    onChange={onScream}
                    className="mb-3"
                    checked={allows.scream}
                    type="switch"
                    label="Скример"
                    name="scream"
                    id="scream"
                />
                <Form.Check
                    onChange={onChange}
                    checked={allows.change}
                    type="switch"
                    label="Смена имени"
                    name="change"
                    id="change"
                />
            </Form>
        </ContentBlock>
    )
};

export default UserAllowsModule;