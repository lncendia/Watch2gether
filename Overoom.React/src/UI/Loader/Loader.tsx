import { DotLoader } from "react-spinners";
import styles from "./Loader.module.css"

type Props = {
    isLoading?: boolean;
};

const Loader = ({ isLoading = true }: Props) => {
    return (
        <>
            <div className={styles.spinner}>
                <DotLoader
                    color='#1976d2'
                    loading={isLoading}
                    size={30}
                    aria-label="Loading Spinner"
                    data-testid="Loader"
                />
            </div>
        </>
    );
};

export default Loader;