import {ReactNode} from "react";
import styles from "./KeyList.module.css";

export interface FilmKeyProps {
    title: string,
    values: [string, string?][]
    onKeySelect?: (key: string) => void,
    className?: string
}

interface ValueProps {
    onClick?: () => void,
    className: string
}

const KeyList = ({title, values, onKeySelect, className}: FilmKeyProps) => {

    const valuesNodes: ReactNode[] = [];

    for (let _i = 0; _i < values.length; _i++) {
        const coma = _i !== values.length - 1;
        const valueProps: ValueProps = {
            onClick: undefined,
            className: styles.value
        }

        if (onKeySelect !== undefined) {
            valueProps.onClick = () => onKeySelect(values[_i][0])
            valueProps.className += ` ${styles.clickable}`
        }

        valuesNodes.push(
            <div key={values[_i][0]} className={styles.block}>
                <span {...valueProps}>{values[_i][0]}</span>
                {values[_i][1] && <span className={styles.description}> ({values[_i][1]})</span>}
                {coma && <span>, </span>}
            </div>
        )
    }

    return (
        <div className={className}>
            <span>{title}</span>
            {valuesNodes.map(v => v)}
        </div>
    )
};

export default KeyList;