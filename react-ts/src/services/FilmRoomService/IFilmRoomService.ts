import {SyncEvent} from "ts-events";
import {
    ActionEvent,
    ChangeNameEvent,
    ChangeSeriesEvent,
    MessageEvent,
    MessagesEvent,
    PauseEvent,
    SeekEvent
} from "./Models/RoomEvents.ts";

export interface IFilmRoomService {

    roomLoadEvent: SyncEvent<FilmRoom>;

    connectEvent: SyncEvent<FilmViewer>

    disconnectEvent: SyncEvent<string>

    leaveEvent: SyncEvent<string>

    messagesEvent: SyncEvent<MessagesEvent>

    messageEvent: SyncEvent<MessageEvent>

    beepEvent: SyncEvent<ActionEvent>

    screamEvent: SyncEvent<ActionEvent>

    changeNameEvent: SyncEvent<ChangeNameEvent>

    errorEvent: SyncEvent<string>

    pauseEvent: SyncEvent<PauseEvent>

    seekEvent: SyncEvent<SeekEvent>

    changeSeriesEvent: SyncEvent<ChangeSeriesEvent>

    typeEvent: SyncEvent<string>

    connect(roomId: string, url: string): Promise<void>;

    disconnect(): Promise<void>;

    getMessages(fromId?: string, count?: number): Promise<void>;

    changeSeries(season: number, series: number): Promise<void>;

    sendMessage(message: string): Promise<void>;

    type(): Promise<void>;

    setTimeLine(second: number): Promise<void>;

    setPause(pause: boolean, seconds: number): Promise<void>;

    setFullScreen(fullScreen: boolean): Promise<void>;

    beep(target: string): Promise<void>;

    scream(target: string): Promise<void>;

    kick(target: string): Promise<void>;

    changeName(target: string, name: string): Promise<void>;

    leave(): Promise<void>;
}