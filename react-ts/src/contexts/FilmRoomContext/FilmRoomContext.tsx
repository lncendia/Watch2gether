import React, {createContext, useContext, useState, useEffect, ReactNode} from 'react';
import {useInjection} from "inversify-react";
import {IFilmRoomService} from "../../services/FilmRoomService/IFilmRoomService.ts";
import Loader from "../../UI/Loader/Loader.tsx";
import {FilmRoomData} from "./FilmRoomData.ts";


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
    url: string,
    id: string
}

const map = (v: FilmViewer): FilmViewerData => {
    return {...v, typing: false}
}

export const FilmRoomContextProvider: React.FC<FilmRoomContextProviderProps> = ({id, url, children}) => {

    const [viewers, setViewers] = useState<FilmViewerData[]>()
    const [room, setRoom] = useState<FilmRoomData>()

    const service = useInjection<IFilmRoomService>('FilmRoomService');

    useEffect(() => {
        service.roomLoad.attach((room) => {
            setViewers(room.viewers.map(map))
            setRoom(room)
        })
        service.connect(id, url).then()
    }, [id, service, url]);


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