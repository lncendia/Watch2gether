import BlockTitle from "../../UI/BlockTitle/BlockTitle.tsx";
import UserRatingsModule from "../../modules/Profile/UserRatingsModule/UserRatingsModule.tsx";
import UserInfoModule from "../../modules/Profile/UserInfoModule/UserInfoModule.tsx";
import AuthorizeModule from "../../modules/Authorization/AuthorizeModule.tsx";
import ProfileContextProvider from "../../contexts/ProfileContext/ProfileContext.tsx";
import UserWatchlistModule from "../../modules/Profile/UserWatchlistModule/UserWatchlistModule.tsx";
import UserHistoryModule from "../../modules/Profile/UserHistoryModule/UserHistoryModule.tsx";
import {Col, Row} from "react-bootstrap";
import UserAllowsModule from "../../modules/Profile/UserAllowsModule/UserAllowsModule.tsx";
import ProfileSettingsModule from "../../modules/Profile/ProfileSettingsModule/ProfileSettingsModule.tsx";

const ProfilePage = () => {

    return (
        <AuthorizeModule showError>
            <ProfileContextProvider>
                <Row gx={5}>
                    <Col xl={8} lg={7} md={12} className="order-last order-lg-first">
                        <UserInfoModule className="mt-5"/>
                        <BlockTitle className="mt-5" title="Смотреть позже"/>
                        <UserWatchlistModule/>
                        <BlockTitle className="mt-5" title="История"/>
                        <UserHistoryModule/>
                        <BlockTitle className="mt-5" title="Оценки"/>
                        <UserRatingsModule/>
                    </Col>
                    <Col xl={4} lg={5} md={12} className="order-first order-lg-last">
                        <UserAllowsModule className="mt-5"/>
                        <ProfileSettingsModule className="mt-2"/>
                    </Col>
                </Row>
            </ProfileContextProvider>
        </AuthorizeModule>
    )
};

export default ProfilePage;