import React from 'react';
import styles from "./Svg.module.css";

interface SvgParams {
    width: number,
    height: number,
    fill: string,
    children: React.ReactNode,
    viewBox: string,
    className?: string
    onClick?: React.MouseEventHandler<SVGSVGElement>
}

const Svg = (props: SvgParams) => {
    return (
        <svg onClick={props.onClick} xmlns="http://www.w3.org/2000/svg" width={props.width} height={props.height}
             fill={props.fill}
             className={`${props.className} ${styles.svg}`} viewBox={props.viewBox}>
            {props.children}
        </svg>
    );
};

export default Svg;