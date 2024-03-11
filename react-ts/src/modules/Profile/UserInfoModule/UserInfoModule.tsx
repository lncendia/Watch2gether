import {useUser} from "../../../contexts/UserContext.tsx";
import UserInfo from "../../../components/Profile/UserInfo/UserInfo.tsx";
import {useNavigate} from "react-router-dom";
import {Col} from "react-bootstrap";

const UserInfoModule = ({genres, className}: { genres: string[], className?: string }) => {

    const {authorizedUser} = useUser()

    // Навигационный хук
    const navigate = useNavigate();

    const onGenreSelect = (genre: string) => {
        navigate('/search', {state: {genre: genre}})
    }

    return (
        <Col lg={8} xl={7} className={className}>
            <UserInfo genres={genres} user={authorizedUser!} onGenreSelect={onGenreSelect} className={className}/>
        </Col>
    );
};

export default UserInfoModule;