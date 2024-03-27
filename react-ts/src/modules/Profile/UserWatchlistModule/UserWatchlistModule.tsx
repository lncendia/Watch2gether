import UserFilmsModule from "../UserFilmsModule/UserFilmsModule.tsx";
import {useProfile} from "../../../contexts/ProfileContext/ProfileContext.tsx";


const UserWatchlistModule = ({className}: { className?: string }) => {

    const {watchlist} = useProfile()

    return <UserFilmsModule className={className} films={watchlist}/>;
};

export default UserWatchlistModule;