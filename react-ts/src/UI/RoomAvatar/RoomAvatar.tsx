import styles from "./RoomAvatar.module.css";

const RoomAvatar = ({src, owner, className = ''}: { owner: boolean, src: string, className?: string }) => {
    return (
        <img className={`${styles.avatar} ${className} ${owner ? styles.owner : ''}`} src={src} alt="Аватар"/>
    );
};

export default RoomAvatar;