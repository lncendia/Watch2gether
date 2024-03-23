import ConnectUrl from "../../../../components/FilmRoom/ConnectUrl/ConnectUrl.tsx";
import {useState} from "react";

const ConnectLinkModule = ({id, endpoint, code}: { code?: string, id: string, endpoint: string }) => {

    const [isClicked, setIsClicked] = useState(false)

    const callback = () => {

        const searchParams = new URLSearchParams();

        searchParams.set('id', id);

        if (code) searchParams.set('code', code);

        // формируем новый URL-адрес
        const newUrl = `${window.location.origin}/${endpoint}?${searchParams.toString()}`;

        navigator.clipboard.writeText(newUrl).then()
        setIsClicked(true)

        setTimeout(() => {
            setIsClicked(false)
        }, 5000)
    }

    return (
        <ConnectUrl isClicked={isClicked} onClick={callback}/>
    );
};

export default ConnectLinkModule;