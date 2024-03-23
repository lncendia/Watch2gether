import styles from './Badge.module.css'
import {BadgeData} from "./BadgeData.ts";

const Badge = ({color, text}: BadgeData) => {
    return (
        <div className={`bg-${color} ${styles.badge}`}>
            {text}
        </div>
    );
};

export default Badge;