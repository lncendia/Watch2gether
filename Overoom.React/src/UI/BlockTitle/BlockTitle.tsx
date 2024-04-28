import styles from './BlockTitle.module.css'

const BlockTitle = ({title, className = ''}: { title: string, className?: string }) => {
    return (
        <h4 className={`${styles.title} ${className}`.trim()}>
            {title}
        </h4>
    );
};

export default BlockTitle;