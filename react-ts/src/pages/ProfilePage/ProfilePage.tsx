import ProfileModule from "../../modules/Profile/ProfileModule/ProfileModule.tsx";
import BlockTitle from "../../UI/BlockTitle/BlockTitle.tsx";
import UserRatingsModule from "../../modules/Profile/UserRatingsModule/UserRatingsModule.tsx";


const ProfilePage = () => {
    return (
        <>
            <ProfileModule/>
            <BlockTitle className="mt-5" title="Оценки"/>
            <UserRatingsModule/>
        </>
    )
};

export default ProfilePage;