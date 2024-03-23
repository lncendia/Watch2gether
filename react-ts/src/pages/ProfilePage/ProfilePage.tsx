import BlockTitle from "../../UI/BlockTitle/BlockTitle.tsx";
import UserRatingsModule from "../../modules/Profile/UserRatingsModule/UserRatingsModule.tsx";
import {useEffect, useState} from "react";
import {Profile} from "../../services/ProfileService/Models/Profile.ts";
import {useInjection} from "inversify-react";
import {IProfileService} from "../../services/ProfileService/IProfileService.ts";
import Loader from "../../UI/Loader/Loader.tsx";
import UserInfoModule from "../../modules/Profile/UserInfoModule/UserInfoModule.tsx";
import UserFilmsModule from "../../modules/Profile/UserFilmsModule/UserFilmsModule.tsx";
const ProfilePage = () => {
    
    const [profile, setProfile] = useState<Profile>();
    const profileService = useInjection<IProfileService>('ProfileService');

    useEffect(() => {
        const processProfile = async () => {
            const response = await profileService.profile()
            setProfile(response)
        };

        processProfile().then()
    }, [profileService]);

    if (!profile) return <Loader/>

    return (
        <>
            <UserInfoModule genres={profile.genres} className="mt-5"/>
            <BlockTitle className="mt-5" title="Смотреть позже"/>
            <UserFilmsModule films={profile.watchlist}/>
            <BlockTitle className="mt-5" title="История"/>
            <UserFilmsModule films={profile.history}/>
            <BlockTitle className="mt-5" title="Оценки"/>
            <UserRatingsModule/>
        </>
    )
};

export default ProfilePage;