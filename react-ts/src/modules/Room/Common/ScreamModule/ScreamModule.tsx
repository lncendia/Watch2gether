import Screamer from "../../../../components/Room/Common/Screamer/Screamer.tsx";
import {useCallback, useEffect, useRef, useState} from "react";
import {useFilmRoom} from "../../../../contexts/FilmRoomContext/FilmRoomContext.tsx";

const ScreamModule = () => {

    const [className, setClassName] = useState('d-none')
    const screamer = useRef<HTMLVideoElement>(null)

    const {service, room} = useFilmRoom()

    const onScream = useCallback(() => {
        if (!screamer.current) return
        setClassName('')
        screamer.current.volume = 0.1;
        screamer.current.play().then()
        setTimeout(() => setClassName('d-none'), 2000)
    }, [])

    useEffect(() => {
        service.screamEvent.attach((a) => {
            if (a.target === room.currentId) onScream()
        })

    }, [onScream, room.currentId, service.screamEvent]);

    return (
        <Screamer className={className} src="/video/screamer.mp4" reference={screamer}/>
    );
};

export default ScreamModule;