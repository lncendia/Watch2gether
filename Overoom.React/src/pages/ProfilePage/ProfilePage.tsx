import BlockTitle from "../../UI/BlockTitle/BlockTitle.tsx";
import UserRatingsModule from "../../modules/Profile/UserRatingsModule/UserRatingsModule.tsx";
import UserInfoModule from "../../modules/Profile/UserInfoModule/UserInfoModule.tsx";
import AuthorizeModule from "../../modules/Authorization/AuthorizeModule.tsx";
import ProfileContextProvider from "../../contexts/ProfileContext/ProfileContext.tsx";
import UserWatchlistModule from "../../modules/Profile/UserWatchlistModule/UserWatchlistModule.tsx";
import UserHistoryModule from "../../modules/Profile/UserHistoryModule/UserHistoryModule.tsx";

const ProfilePage = () => {

    return (
        <AuthorizeModule showError>
            <ProfileContextProvider>
                <UserInfoModule className="mt-5"/>
                <BlockTitle className="mt-5" title="Смотреть позже"/>
                <UserWatchlistModule/>
                <BlockTitle className="mt-5" title="История"/>
                <UserHistoryModule/>
                <BlockTitle className="mt-5" title="Оценки"/>
                <UserRatingsModule/>
            </ProfileContextProvider>
        </AuthorizeModule>
    )
};

export default ProfilePage;