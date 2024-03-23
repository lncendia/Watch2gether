import styles from './Badge.module.css'

export interface BadgeProps {
    color: 'primary' | 'secondary' | 'warning' | 'danger' | 'dark' | 'light'
    text: string
}

const Badge = ({color, text}: BadgeProps) => {
    return (
        <div className={`bg-${color} ${styles.badge}`}>
            {text}
        </div>
    );
};

export default Badge;