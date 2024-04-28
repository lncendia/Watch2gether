import styles from './Wishes.module.css'

const Wishes = ({text, className = ''}: { text: string, className?: string }) => {
    return (
        <div className={`${styles.block} ${className}`.trim()}>
            <span className={styles.text}>{text}</span>
        </div>
    );
};

export default Wishes;