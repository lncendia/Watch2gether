import React from 'react';
import styles from './Spinner.module.css'

const Spinner = () => {
    return (
        <div className={styles.spinner}>
            <div className={`spinner-grow text-primary ${styles.spinner_grow}`} role="status">
                <span className="visually-hidden">Loading...</span>
            </div>
            <div className={`spinner-grow text-secondary ${styles.spinner_grow}`} role="status">
                <span className="visually-hidden">Loading...</span>
            </div>
            <div className={`spinner-grow text-success ${styles.spinner_grow}`} role="status">
                <span className="visually-hidden">Loading...</span>
            </div>
            <div className={`spinner-grow text-danger ${styles.spinner_grow}`} role="status">
                <span className="visually-hidden">Loading...</span>
            </div>
            <div className={`spinner-grow text-warning ${styles.spinner_grow}`} role="status">
                <span className="visually-hidden">Loading...</span>
            </div>
            <div className={`spinner-grow text-info ${styles.spinner_grow}`} role="status">
                <span className="visually-hidden">Loading...</span>
            </div>
            <div className={`spinner-grow text-light ${styles.spinner_grow}`} role="status">
                <span className="visually-hidden">Loading...</span>
            </div>
            <div className={`spinner-grow text-dark ${styles.spinner_grow}`} role="status">
                <span className="visually-hidden">Loading...</span>
            </div>
        </div>
    );
};

export default Spinner;