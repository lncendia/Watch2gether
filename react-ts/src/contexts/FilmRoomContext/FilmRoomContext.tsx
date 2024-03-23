import React, {createContext, useContext, useState, useEffect, ReactNode, useRef} from 'react';
import {useInjection} from "inversify-react";
import {IFilmRoomService} from "../../services/FilmRoomService/IFilmRoomService.ts";
import Loader from "../../UI/Loader/Loader.tsx";
import {FilmRoomData} from "./FilmRoomData.ts";
import {FilmRoom} from "../../services/RoomsService/Models/Rooms.ts";
import {RoomServer} from "../../services/RoomsService/Models/RoomServer.ts";
import {useUser} from "../UserContext/UserContext.tsx";


// Создайте интерфейс для контекста
interface FilmRoomContextData {
    viewers: FilmViewerData[];
    room: FilmRoomData;
    service: IFilmRoomService;
    updateTimeLine: (id: string, second: number) => void
    updatePause: (id: string, pause: boolean, second: number) => void
    changeSeries: (id: string, season: number, series: number) => void
}

// Создайте сам контекст
const FilmRoomContext = createContext<FilmRoomContextData | undefined>(undefined);

// Создайте провайдер
interface FilmRoomContextProviderProps {
    children: ReactNode;
    filmRoom: FilmRoom
    server: RoomServer
}

const map = (v: FilmViewer): FilmViewerData => {
    return {...v, typing: false}
}

export const FilmRoomContextProvider: React.FC<FilmRoomContextProviderProps> = ({filmRoom, server, children}) => {

    const [viewers, setViewers] = useState<FilmViewerData[]>()
    const [room, setRoom] = useState<FilmRoomData>()
    const {authorizedUser} = useUser()
    const userId = useRef(authorizedUser!.id)

    const service = useInjection<IFilmRoomService>('FilmRoomService');

    useEffect(() => {
        service.roomLoadEvent.attach(async (room) => {
            setViewers(room.viewers.map(map))
            setRoom({...room, ...filmRoom, ...server, currentId: userId.current})
        })
        service.connect(server.id, server.url).then()
    }, [filmRoom, server, server.id, server.url, service]);


    const updateTimeLine = (id: string, second: number) => {
        setViewers(prevState => {
            const viewer = viewers!.filter(v => v.id === id)[0];
            viewer.second = second
            return prevState!.map(v => v)
        })
    }

    const updatePause = (id: string, pause: boolean, second: number) => {
        setViewers(prevState => {
            const viewer = viewers!.filter(v => v.id === id)[0];
            viewer.pause = pause
            viewer.second = second
            return prevState!.map(v => v)
        })
    }

    const changeSeries = (id: string, season: number, series: number) => {
        setViewers(prevState => {
            const viewer = viewers!.filter(v => v.id === id)[0];
            viewer.season = season
            viewer.series = series
            viewer.second = 0
            viewer.pause = true
            return prevState!.map(v => v)
        })
    }

    if (!room || !viewers) return <Loader/>

    return (
        <FilmRoomContext.Provider value={{room, viewers, service, updatePause, updateTimeLine, changeSeries}}>
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