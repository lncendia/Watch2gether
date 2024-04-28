import {useCallback, useEffect, useRef, useState} from "react";
import {useFilmRoom} from "../../../../contexts/FilmRoomContext/FilmRoomContext.tsx";
import Badge, {BadgeProps} from "../../../../components/Room/Common/Badge/Badge.tsx";

interface Notification extends BadgeProps {
    timeoutId: NodeJS.Timeout
}

const NotificationModule = () => {
    const {viewers, service} = useFilmRoom();
    const [notification, setNotification] = useState<Notification>();
    const viewersRef = useRef(viewers);

    useEffect(() => {
        viewersRef.current = viewers;
    }, [viewers]);

    const showNotification = useCallback((color: 'primary' | 'secondary' | 'warning' | 'danger' | 'dark' | 'light', text: string) => {

        const timeOut = setTimeout(() => setNotification(prev => {
            if (prev) return {...prev, open: false}
        }), 6000);

        setNotification(prev => {
            if (prev) clearTimeout(prev.timeoutId);
            return {color: color, text: text, timeoutId: timeOut, open: true};
        });
    }, []);

    useEffect(() => {

        const getViewer = (id: string) => {
            return viewersRef.current.filter(v => v.id === id)[0];
        };

        service.beepEvent.attach((action) => {
            const initiator = getViewer(action.initiator);
            const target = getViewer(action.target);
            showNotification("primary", `${initiator.username} разбудил ${target.username}`);
        });

        service.connectEvent.attach((viewer) => {
            showNotification("primary", `${viewer.username} подключился`);
        });

        service.disconnectEvent.attach((id) => {
            const initiator = getViewer(id);
            showNotification("primary", `${initiator.username} отключился`);
        });

        service.leaveEvent.attach((id) => {
            const initiator = getViewer(id);
            showNotification("secondary", `${initiator.username} покинул комнату`);
        });

        service.screamEvent.attach((action) => {
            const initiator = getViewer(action.initiator);
            const target = getViewer(action.target);
            showNotification("primary", `${initiator.username} напугал ${target.username}`);
        });

        service.errorEvent.attach((error) => {
            showNotification("danger", error);
        });
    }, [showNotification, service.beepEvent, service.errorEvent, service.screamEvent]);

    if (!notification) return <></>;

    return (
        <Badge {...notification}/>
    );
}

export default NotificationModule;