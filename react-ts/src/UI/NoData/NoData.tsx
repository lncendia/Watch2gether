import React from 'react';
import ContentBlock from "../ContentBlock/ContentBlock.tsx";

const NoData = ({text, className = ''}: { text: string, className?: string }) => {
    return (
        <ContentBlock className={`text-center ${className}`.trim()}>
            <div className="my-1 fs-5">{text}</div>
        </ContentBlock>
    );
};

export default NoData;