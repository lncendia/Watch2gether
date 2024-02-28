import { DotLoader } from "react-spinners";
import "./Loader.module.scss";

type Props = {
    isLoading?: boolean;
};

const Loader = ({ isLoading = true }: Props) => {
    return (
        <>
            <div id='loading-spinner'>
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