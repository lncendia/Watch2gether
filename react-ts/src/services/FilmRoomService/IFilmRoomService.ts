import {SyncEvent} from "ts-events";
import {ActionEvent, MessageEvent, MessagesEvent} from "./Models/RoomEvents.ts";

export interface IFilmRoomService {

    roomLoadEvent: SyncEvent<FilmRoom>;

    messagesEvent: SyncEvent<MessagesEvent>

    messageEvent: SyncEvent<MessageEvent>
    
    beepEvent: SyncEvent<ActionEvent>

    screamEvent: SyncEvent<ActionEvent>

    connect(roomId: string, url:string): Promise<void>;

    getMessages(fromId?: string, count?: number): Promise<void>;

    changeSeries(season: number, series: number): Promise<void>;

    sendMessage(message: string): Promise<void>;

    setTimeLine(second: number): Promise<void>;

    setPause(pause: boolean, seconds: number): Promise<void>;

    setFullScreen(fullScreen: boolean): Promise<void>;

    beep(target: string): Promise<void>;

    scream(target: string): Promise<void>;

    kick(target: string): Promise<void>;

    changeName(target: string, name: string): Promise<void>;
}