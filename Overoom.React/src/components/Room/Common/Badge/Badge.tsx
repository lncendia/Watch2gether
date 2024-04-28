import styles from './Badge.module.css'

export interface BadgeProps {
    color: 'primary' | 'secondary' | 'warning' | 'danger' | 'dark' | 'light'
    text: string
    open: boolean
}

const Badge = ({color, text, open}: BadgeProps) => {

    return (
        <div className={`bg-${color} ${styles.badge} ${open ? '' : styles.off}`}>
            {text}
        </div>
    );
};

export default Badge;