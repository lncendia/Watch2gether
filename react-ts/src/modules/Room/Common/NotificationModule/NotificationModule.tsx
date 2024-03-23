import {useCallback, useEffect, useRef, useState} from "react";
import {useFilmRoom} from "../../../../contexts/FilmRoomContext/FilmRoomContext.tsx";
import Badge from "../../../../components/FilmRoom/Badge/Badge.tsx";


interface BadgeData {
    color: 'primary' | 'secondary' | 'warning' | 'danger' | 'dark' | 'light'
    text: string,
    timeoutId: NodeJS.Timeout
}

const NotificationModule = () => {

    const {viewers, service} = useFilmRoom()
    const [badge, setBadge] = useState<BadgeData>()
    const viewersRef = useRef(viewers)


    const getViewer = useCallback((id: string) => {
        return viewersRef.current.filter(v => v.id === id)[0];
    }, [])

    const initBadge = useCallback((color: 'primary' | 'secondary' | 'warning' | 'danger' | 'dark' | 'light', text: string) => {
        const timeOut = setTimeout(() => setBadge(undefined), 6000)
        setBadge(prev => {
            if (prev) clearTimeout(prev.timeoutId);
            return {color: color, text: text, timeoutId: timeOut}
        })
    }, [])

    useEffect(() => {

        service.beepEvent.attach((action) => {
            const initiator = getViewer(action.initiator)
            const target = getViewer(action.target)
            initBadge("primary", `${initiator.username} разбудил ${target.username}`)
        })

        service.screamEvent.attach((action) => {
            const initiator = getViewer(action.initiator)
            const target = getViewer(action.target)
            initBadge("primary", `${initiator.username} напугал ${target.username}`)
        })

        service.errorEvent.attach((error) => {
            initBadge("danger", error)
        })

    }, [getViewer, initBadge, service.beepEvent, service.errorEvent, service.screamEvent]);


    if (!badge) return <></>

    return (
        <Badge color={badge.color} text={badge.text}/>
    );
};

export default NotificationModule;