import {createContext, ReactNode, useCallback, useContext, useEffect, useState} from 'react';
import {Allows, Profile, UserFilm} from "../../services/ProfileService/ViewModels/ProfileViewModels.ts";
import {useInjection} from "inversify-react";
import {IProfileService} from "../../services/ProfileService/IProfileService.ts";
import Loader from "../../UI/Loader/Loader.tsx";

interface ProfileContextType {
    allows: Allows;
    watchlist: UserFilm[];
    history: UserFilm[];
    genres: string[];
    changeAllows: (allows: Allows) => void
}

// Создайте сам контекст
const ProfileContext = createContext<ProfileContextType | undefined>(undefined);


const ProfileContextProvider = ({children}: { children: ReactNode }) => {
    const [profile, setProfile] = useState<Profile>();
    const profileService = useInjection<IProfileService>('ProfileService');

    useEffect(() => {
        const processProfile = async () => {
            const response = await profileService.profile()
            setProfile(response)
        };

        processProfile().then()
    }, [profileService]);


    const changeAllows = useCallback((allows: Allows) => {
        setProfile(prev => {
            if (prev) return {...prev, allows: allows}
        })
    }, [])

    if (!profile) return <Loader/>

    return (
        <ProfileContext.Provider value={{...profile, changeAllows}}>
            {children}
        </ProfileContext.Provider>
    );
};

// Хук для использования контекста
export const useProfile = () => {
    const context = useContext(ProfileContext);
    if (context === undefined) {
        throw new Error('useProfile must be used within a ProfileContext');
    }
    return context;
};

export default ProfileContextProvider;