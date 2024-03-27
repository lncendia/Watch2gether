import {useCallback, useEffect, useRef, useState} from "react";
import {useFilmRoom} from "../../../../contexts/FilmRoomContext/FilmRoomContext.tsx";
import Badge from "../../../../components/Room/Common/Badge/Badge.tsx";


interface BadgeData {
    color: 'primary' | 'secondary' | 'warning' | 'danger' | 'dark' | 'light'
    text: string,
    timeoutId: NodeJS.Timeout
}

const NotificationModule = () => {

    const {viewers, service} = useFilmRoom()
    const [badge, setBadge] = useState<BadgeData>()
    const [badgeOpen, setBadgeOpen] = useState(false)
    const viewersRef = useRef(viewers)


    useEffect(() => {
        viewersRef.current = viewers
    }, [viewers]);

    const initBadge = useCallback((color: 'primary' | 'secondary' | 'warning' | 'danger' | 'dark' | 'light', text: string) => {
        const timeOut = setTimeout(() => setBadgeOpen(false), 6000)
        setBadge(prev => {
            if (prev) clearTimeout(prev.timeoutId);
            return {color: color, text: text, timeoutId: timeOut}
        })
        setBadgeOpen(true)
    }, [])

    useEffect(() => {

        const getViewer = (id: string) => {
            return viewersRef.current.filter(v => v.id === id)[0];
        }

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

        service.leaveEvent.attach((id) => {
            const viewer = getViewer(id)
            initBadge("secondary", `${viewer.username} покинул комнату`)
        })

        service.errorEvent.attach((error) => {
            initBadge("danger", error)
        })

    }, [initBadge, service.beepEvent, service.errorEvent, service.screamEvent]);

    if (!badge) return <></>

    return (
        <Badge open={badgeOpen} color={badge.color} text={badge.text}/>
    );
};

export default NotificationModule;