import UserFilmsModule from "../UserFilmsModule/UserFilmsModule.tsx";
import {useProfile} from "../../../contexts/ProfileContext/ProfileContext.tsx";


const UserHistoryModule = ({className}: { className?: string }) => {

    const {history} = useProfile()

    return <UserFilmsModule className={className} films={history}/>;
};

export default UserHistoryModule;