import {ReactNode} from "react";
import {AuthorizedUser} from "../../../contexts/UserContext/AuthorizedUser.ts";
import ContentBlock from "../../../UI/ContentBlock/ContentBlock.tsx";
import styles from "./UserInfo.module.css"


interface UserInfoProps {
    user: AuthorizedUser,
    genres: string[],
    className?: string,
    onGenreSelect: (genre: string) => void
}

const UserInfo = ({user, genres, className = '', onGenreSelect}: UserInfoProps) => {

    const valuesNodes: ReactNode[] = [];

    for (let _i = 0; _i < genres.length; _i++) {
        let coma = _i !== genres.length - 1;
        valuesNodes.push(
            <div key={genres[_i]} className={styles.block}>
                <span onClick={() => onGenreSelect(genres[_i])} className={styles.genre}>{genres[_i]}</span>
                {coma && <span>, </span>}
            </div>
        )
    }

    return (
        <ContentBlock className={`d-flex align-items-center ms-2 ${className}`.trim()}>
            <img src={user.avatarUrl} alt="Аватар" className={styles.avatar}/>
            <div>
                <h3 className={styles.name}>{user.name}</h3>
                {valuesNodes}
            </div>
        </ContentBlock>
    );
};

export default UserInfo;