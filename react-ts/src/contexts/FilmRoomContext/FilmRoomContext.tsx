import React, {createContext, useContext, useState, useEffect, ReactNode, useRef, useCallback} from 'react';
import {useInjection} from "inversify-react";
import {IFilmRoomService} from "../../services/FilmRoomService/IFilmRoomService.ts";
import Loader from "../../UI/Loader/Loader.tsx";
import {FilmRoomData} from "./FilmRoomData.ts";
import {FilmRoom} from "../../services/RoomsService/Models/Rooms.ts";
import {RoomServer} from "../../services/RoomsService/Models/RoomServer.ts";
import {useUser} from "../UserContext/UserContext.tsx";
import {IFilmRoomServiceFactory} from "../../services/FilmRoomService/IFilmRoomServiceFactory.ts";


// Создайте интерфейс для контекста
interface FilmRoomContextData {
    viewers: FilmViewerData[];
    viewersParams: FilmViewerParams[];
    room: FilmRoomData;
    service: IFilmRoomService;
    connectViewer: (viewer: FilmViewer) => void
    disconnectViewer: (id: string) => void
    removeViewer: (id: string) => void
    changeName: (id: string, name: string) => void
    updateTimeLine: (id: string, second: number) => void
    updatePause: (id: string, pause: boolean, second: number) => void
    changeSeries: (id: string, season: number, series: number) => void
    type: (id: string) => void
}

// Создайте сам контекст
const FilmRoomContext = createContext<FilmRoomContextData | undefined>(undefined);

// Создайте провайдер
interface FilmRoomContextProviderProps {
    children: ReactNode;
    filmRoom: FilmRoom
    server: RoomServer
}

export const FilmRoomContextProvider: React.FC<FilmRoomContextProviderProps> = ({filmRoom, server, children}) => {

    const [viewers, setViewers] = useState<FilmViewerData[]>([])
    const [viewersParams, setViewersParams] = useState<FilmViewerParams[]>([])
    const [room, setRoom] = useState<FilmRoomData>()
    const [service, setService] = useState<IFilmRoomService>()
    const {authorizedUser} = useUser()
    const userId = useRef(authorizedUser!.id)

    const factory = useInjection<IFilmRoomServiceFactory>('FilmRoomServiceFactory');

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

    const connectViewer = useCallback((viewer: FilmViewer) => {
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
        <FilmRoomContext.Provider
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
        </FilmRoomContext.Provider>
    );
}

// Хук для использования контекста
export const useFilmRoom = () => {
    const context = useContext(FilmRoomContext);
    if (context === undefined) {
        throw new Error('useFilmRoom must be used within a FilmRoomContext');
    }
    return context;
};