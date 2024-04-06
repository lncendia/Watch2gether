import {useCallback, useEffect, useRef, useState} from "react";
import {useFilmRoom} from "../../../../contexts/FilmRoomContext/FilmRoomContext.tsx";
import Badge from "../../../../components/Room/Common/Badge/Badge.tsx";

const NotificationModule = () => {
    const {viewers, service} = useFilmRoom();
    const [notification, setNotification] = useState<any>();
    const [notificationOpen, setNotificationOpen] = useState(false);
    const viewersRef = useRef(viewers);

    useEffect(() => {
        viewersRef.current = viewers;
    }, [viewers]);

    const showNotification = useCallback((color: 'primary' | 'secondary' | 'warning' | 'danger' | 'dark' | 'light', text: string) => {
        const timeOut = setTimeout(() => setNotificationOpen(false), 6000);
        setNotification(prev => {
            if (prev) clearTimeout(prev.timeoutId);
            return {color: color, text: text, timeoutId: timeOut};
        });
        setNotificationOpen(true);
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
            console.log("тута")
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
        <Badge open={notificationOpen} color={notification.color} text={notification.text}/>
    );
}

export default NotificationModule;