import BlockTitle from "../../UI/BlockTitle/BlockTitle.tsx";
import UserRatingsModule from "../../modules/Profile/UserRatingsModule/UserRatingsModule.tsx";
import UserInfoModule from "../../modules/Profile/UserInfoModule/UserInfoModule.tsx";
import AuthorizeModule from "../../modules/Authorization/AuthorizeModule.tsx";
import ProfileContextProvider from "../../contexts/ProfileContext/ProfileContext.tsx";
import UserWatchlistModule from "../../modules/Profile/UserWatchlistModule/UserWatchlistModule.tsx";
import UserHistoryModule from "../../modules/Profile/UserHistoryModule/UserHistoryModule.tsx";
import {Col} from "react-bootstrap";

const ProfilePage = () => {

    return (
        <AuthorizeModule showError>
            <ProfileContextProvider>
                <Col lg={8} xl={7} className="mt-5">
                    <UserInfoModule className="mt-5"/>
                </Col>
                <BlockTitle className="mt-5" title="Смотреть позже"/>
                <Col xl={9} lg={10} className="mt-5">
                    <UserWatchlistModule/>
                </Col>
                <BlockTitle className="mt-5" title="История"/>
                <Col xl={9} lg={10} className="mt-5">
                    <UserHistoryModule/>
                </Col>
                <BlockTitle className="mt-5" title="Оценки"/>
                <UserRatingsModule/>
            </ProfileContextProvider>
        </AuthorizeModule>
    )
};

export default ProfilePage;