import {useEffect, useState} from "react";
import {useInjection} from "inversify-react";
import {Profile} from "../../../services/ProfileService/Models/Profile.ts";
import {IProfileService} from "../../../services/ProfileService/IProfileService.ts";
import UserFilmsModule from "../UserFilmsModule/UserFilmsModule.tsx";
import Loader from "../../../UI/Loader/Loader.tsx";
import UserInfoModule from "../UserInfoModule/UserInfoModule.tsx";

const ProfileModule = () => {

    const [profile, setProfile] = useState<Profile>();
    const profileService = useInjection<IProfileService>('ProfileService');

    useEffect(() => {
        const processProfile = async () => {
            const response = await profileService.profile()
            setProfile(response)
        };

        processProfile().then()
    }, []);

    if (!profile) return <Loader/>

    return (
        <>
            <UserInfoModule genres={profile.genres} className="mt-5"/>
            <UserFilmsModule className="mt-5" films={profile.watchlist} title={"Смотреть позже"}/>
            <UserFilmsModule className="mt-5" films={profile.history} title={"История"}/>
        </>
    );
};

export default ProfileModule;