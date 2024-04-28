import {useCallback, useEffect, useRef} from "react";
import {useFilmRoom} from "../../../../contexts/FilmRoomContext/FilmRoomContext.tsx";

const BeepModule = () => {

    const beep = useRef<HTMLAudioElement>(null)

    const {service, room} = useFilmRoom()

    const onBeep = useCallback(() => {
        if (!beep.current) return
        beep.current.currentTime = 0
        beep.current.volume = 0.3;
        beep.current.play().then()
    }, [])

    useEffect(() => {
        service.beepEvent.attach((a) => {
            if (a.target === room.currentId) onBeep()
        })

    }, [onBeep, room.currentId, service.beepEvent]);

    return (
        <audio ref={beep} className="d-none" src="/audio/beep.mp3"/>
    );
};

export default BeepModule;