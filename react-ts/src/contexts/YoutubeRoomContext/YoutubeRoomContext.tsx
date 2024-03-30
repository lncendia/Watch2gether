import React, {createContext, useContext, useState, useEffect, ReactNode, useRef, useCallback} from 'react';
import {useInjection} from "inversify-react";
import {IYoutubeRoomService} from "../../services/YoutubeRoomService/IYoutubeRoomService.ts";
import Loader from "../../UI/Loader/Loader.tsx";
import {YoutubeRoomData} from "./YoutubeRoomData.ts";
import {YoutubeRoom} from "../../services/RoomsService/Models/Rooms.ts";
import {RoomServer} from "../../services/RoomsService/Models/RoomServer.ts";
import {useUser} from "../UserContext/UserContext.tsx";
import {IYoutubeRoomServiceFactory} from "../../services/YoutubeRoomService/IYoutubeRoomServiceFactory.ts";


// Создайте интерфейс для контекста
interface YoutubeRoomContextData {
    viewers: YoutubeViewerData[];
    viewersParams: YoutubeViewerParams[];
    room: YoutubeRoomData;
    service: IYoutubeRoomService;
    connectViewer: (viewer: YoutubeViewer) => void
    disconnectViewer: (id: string) => void
    removeViewer: (id: string) => void
    changeName: (id: string, name: string) => void
    updateTimeLine: (id: string, second: number) => void
    updatePause: (id: string, pause: boolean, second: number) => void
    changeSeries: (id: string, season: number, series: number) => void
    type: (id: string) => void
}

// Создайте сам контекст
const YoutubeRoomContext = createContext<YoutubeRoomContextData | undefined>(undefined);

// Создайте провайдер
interface YoutubeRoomContextProviderProps {
    children: ReactNode;
    filmRoom: YoutubeRoom
    server: RoomServer
}

export const YoutubeRoomContextProvider: React.FC<YoutubeRoomContextProviderProps> = ({filmRoom, server, children}) => {

    const [viewers, setViewers] = useState<YoutubeViewerData[]>([])
    const [viewersParams, setViewersParams] = useState<YoutubeViewerParams[]>([])
    const [room, setRoom] = useState<YoutubeRoomData>()
    const [service, setService] = useState<IYoutubeRoomService>()
    const {authorizedUser} = useUser()
    const userId = useRef(authorizedUser!.id)

    const factory = useInjection<IYoutubeRoomServiceFactory>('YoutubeRoomServiceFactory');

    useEffect(() => {

        const service = factory.create()

        service.roomLoadEvent.attach(async (room) => {
            setViewers(room.viewers.map(v => {
                return {...v, ...v.allows, photoUrl: v.photoUrl ?? '/img/profile.svg'}
            }))

            setViewersParams(room.viewers.map(v => {
                return {...v, typing: false}
            }))

            setRoom({...room, ...filmRoom, ...server, currentId: userId.current})
        })
        service.connect(server.id, server.url).then()

        setService(service)

        return () => {
            service.disconnect().then()
        }
    }, [filmRoom, server, server.id, server.url, factory]);


    const updateTimeLine = useCallback((id: string, second: number) => {
        setViewersParams(prevState => {
            const viewer = prevState!.filter(v => v.id === id)[0];
            viewer.second = second
            return [...prevState]
        })
    }, [])

    const type = useCallback((id: string) => {

        const timeOut = setTimeout(() => {
            setViewersParams(prevState => {
                const viewer = prevState.filter(v => v.id === id)[0];
                viewer.typing = false
                viewer.typingTimeout = undefined
                return [...prevState]
            })
        }, 2000)

        setViewersParams(prevState => {
            const viewer = prevState.filter(v => v.id === id)[0];
            if (viewer.typingTimeout) clearTimeout(viewer.typingTimeout)
            viewer.typing = true
            viewer.typingTimeout = timeOut
            return [...prevState]
        })
    }, [])

    const updatePause = useCallback((id: string, pause: boolean, second: number) => {
        setViewersParams(prevState => {
            const viewer = prevState.filter(v => v.id === id)[0];
            viewer.pause = pause
            viewer.second = second
            return [...prevState]
        })
    }, [])

    const changeSeries = useCallback((id: string, season: number, series: number) => {
        setViewersParams(prevState => {
            const viewer = prevState.filter(v => v.id === id)[0];
            viewer.season = season
            viewer.series = series
            viewer.second = 0
            viewer.pause = true
            return [...prevState]
        })
    }, [])

    const changeName = useCallback((id: string, name: string) => {
        setViewers(prevState => {
            const viewer = prevState.filter(v => v.id === id)[0];
            viewer.username = name
            return [...prevState]
        })
    }, [])

    const connectViewer = useCallback((viewer: YoutubeViewer) => {
        setViewers(prevState => {
            const viewerData = {...viewer, ...viewer.allows, photoUrl: viewer.photoUrl ?? '/img/profile.svg'}
            return [...prevState.filter(v => v.id !== viewer.id), viewerData]
        })

        setViewersParams(prevState => {
            const viewerParams = {...viewer, typing: false}
            return [...prevState.filter(v => v.id !== viewer.id), viewerParams]
        })

    }, [])

    const disconnectViewer = useCallback((id: string) => {
        setViewersParams(prevState => {
            const viewer = prevState.filter(v => v.id === id)[0];
            viewer.online = false;
            return [...prevState]
        })

    }, [])

    const removeViewer = useCallback((id: string) => {
        setViewers(prevState => {
            return [...prevState.filter(v => v.id !== id)]
        })

        setViewersParams(prevState => {
            return [...prevState.filter(v => v.id !== id)]
        })
    }, [])

    useEffect(() => {
        const intervalId = setInterval(() => {
            setViewersParams(prevState => {
                prevState.forEach(v => {
                    if (!v.pause) v.second++
                })
                return [...prevState]
            })
        }, 1000)

        return () => clearInterval(intervalId)
    }, []);

    if (!room || !viewers || !service || !viewersParams) return <Loader/>

    return (
        <YoutubeRoomContext.Provider
            value={{
                room,
                changeName,
                viewers,
                viewersParams,
                service,
                updatePause,
                updateTimeLine,
                changeSeries,
                type,
                connectViewer,
                disconnectViewer,
                removeViewer
            }}>
            {children}
        </YoutubeRoomContext.Provider>
    );
}

// Хук для использования контекста
export const useYoutubeRoom = () => {
    const context = useContext(YoutubeRoomContext);
    if (context === undefined) {
        throw new Error('useYoutubeRoom must be used within a YoutubeRoomContext');
    }
    return context;
};