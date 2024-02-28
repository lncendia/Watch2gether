// Компонент с информацией из заголовка и абзаца
const InfoPanel = ({header, info}: {header:string, info:string | null}) => {

    return(
        <>        
            <h2>{header}</h2>
            {info !== null && <p>{info}</p>}
        </>
    );
}

export default InfoPanel;