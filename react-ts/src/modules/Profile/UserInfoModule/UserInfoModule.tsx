import {useUser} from "../../../contexts/UserContext/UserContext.tsx";
import UserInfo from "../../../components/Profile/UserInfo/UserInfo.tsx";
import {useNavigate} from "react-router-dom";
import {Col} from "react-bootstrap";
import {useCallback} from "react";

const UserInfoModule = ({genres, className}: { genres: string[], className?: string }) => {

    const {authorizedUser} = useUser()

    // Навигационный хук
    const navigate = useNavigate();

    const onGenreSelect = useCallback((genre: string) => {
        navigate('/search', {state: {genre: genre}})
    }, [navigate])

    return (
        <Col lg={8} xl={7} className={className}>
            <UserInfo genres={genres} onGenreSelect={onGenreSelect} className={className}
                      avatar={authorizedUser!.avatarUrl} name={authorizedUser!.name}/>
        </Col>
    );
};

export default UserInfoModule;