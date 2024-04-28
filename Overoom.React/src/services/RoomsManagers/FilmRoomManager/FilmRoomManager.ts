import * as signalR from "@microsoft/signalr";
import {HubConnection} from "@microsoft/signalr";
import {IFilmRoomManager} from "./IFilmRoomManager.ts";
import {SyncEvent} from "ts-events";
import {FilmRoom, FilmViewer} from "./ViewModels/FilmRoomViewModels.ts";
import {
    ActionEvent,
    ChangeNameEvent,
    ChangeSeriesEvent,
    MessagesEvent,
    PauseEvent,
    SeekEvent,
    MessageEvent
} from "./ViewModels/FilmRoomEvents.ts";


export class FilmRoomManager implements IFilmRoomManager {

    private connection?: HubConnection
    private readonly tokenFactory: () => Promise<string>

    private readonly authUrl: string

    roomLoadEvent = new SyncEvent<FilmRoom>();

    connectEvent = new SyncEvent<FilmViewer>

    disconnectEvent = new SyncEvent<string>

    leaveEvent = new SyncEvent<string>

    messagesEvent = new SyncEvent<MessagesEvent>();

    messageEvent = new SyncEvent<MessageEvent>();

    beepEvent = new SyncEvent<ActionEvent>()

    screamEvent = new SyncEvent<ActionEvent>()

    errorEvent = new SyncEvent<string>()

    pauseEvent = new SyncEvent<PauseEvent>()

    seekEvent = new SyncEvent<SeekEvent>()

    typeEvent = new SyncEvent<string>()

    changeNameEvent = new SyncEvent<ChangeNameEvent>

    changeSeriesEvent = new SyncEvent<ChangeSeriesEvent>

    constructor(tokenFactory: () => Promise<string>, authUrl: string) {
        this.authUrl = authUrl
        this.tokenFactory = tokenFactory
    }

    async connect(roomId: string, url: string) {

        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(url + "filmRoom", {accessTokenFactory: this.tokenFactory})
            .configureLogging(signalR.LogLevel.Information)
            .build();

        this.connection.on("Room", (room: FilmRoom) => {
            room.viewers.forEach(v => {
                if (v.photoUrl) v.photoUrl = `${this.authUrl}/${v.photoUrl}`
            })
            this.roomLoadEvent.post(room)
        })

        this.connection.on('Connect', (viewer: FilmViewer) => {
            if (viewer.photoUrl) viewer.photoUrl = `${this.authUrl}/${viewer.photoUrl}`
            this.connectEvent.post(viewer)
        });

        this.connection.on('Disconnect', (id: string) => this.disconnectEvent.post(id));

        this.connection.on('Leave', (id: string) => this.leaveEvent.post(id));

        this.connection.on("Messages", (messages: MessagesEvent) => {
            messages.messages.forEach(m => {
                m.createdAt = new Date(m.createdAt)
            })
            this.messagesEvent.post(messages)
        })

        this.connection.on('NewMessage', (message: MessageEvent) => {
            message.createdAt = new Date(message.createdAt)
            this.messageEvent.post(message)
        });

        this.connection.on('Beep', (action: ActionEvent) =>
            this.beepEvent.post(action));

        this.connection.on('Scream', (action: ActionEvent) =>
            this.screamEvent.post(action));

        this.connection.on('ChangeName', (action: ChangeNameEvent) =>
            this.changeNameEvent.post(action));

        this.connection.on('Error', (error: string) => this.errorEvent.post(error));

        this.connection.on('Pause', (id: string, pause: boolean, second: number) =>
            this.pauseEvent.post({onPause: pause, seconds: second, userId: id}));

        this.connection.on('Seek', (id: string, second: number) =>
            this.seekEvent.post({seconds: second, userId: id}));

        this.connection.on('Type', (id: string) => this.typeEvent.post(id));

        this.connection.on('ChangeSeries', (id: string, season: number, series: number) => {
            this.changeSeriesEvent.post({
                userId: id,
                season: season,
                series: series
            })
        });
        // this.connection.on('Kick', (id, target) => room.ProcessReceiveEvent(new KickReceiveEvent(id, target)));
        // this.connection.on('FullScreen', (id, fullscreen) => room.ProcessReceiveEvent(new FullScreenReceiveEvent(id, fullscreen)));

        await this.connection.start();
        await this.connection.send("Connect", roomId)
    }

    async disconnect(): Promise<void> {
        await this.connection?.stop()
    }

    async getMessages(fromId: string, count: number): Promise<void> {
        await this.connection?.send("GetMessages", fromId, count)
    }


    async changeSeries(season: number, series: number): Promise<void> {
        await this.connection?.send("ChangeSeries", season, series)
    }

    async sendMessage(message: string): Promise<void> {
        await this.connection?.send("SendMessage", message)
    }

    async setTimeLine(seconds: number): Promise<void> {
        await this.connection?.send("SetTimeLine", parseInt(seconds.toString()))
    }

    async setPause(pause: boolean, seconds: number): Promise<void> {
        await this.connection?.send("SetPause", pause, parseInt(seconds.toString()))
    }

    async setFullScreen(fullScreen: boolean): Promise<void> {
        await this.connection?.send("SetFullScreen", fullScreen)
    }

    async beep(target: string): Promise<void> {
        await this.connection?.send("Beep", target)
    }

    async scream(target: string): Promise<void> {
        await this.connection?.send("Scream", target)
    }

    async kick(target: string): Promise<void> {
        await this.connection?.send("Kick", target)
    }

    async changeName(target: string, name: string): Promise<void> {
        await this.connection?.send("ChangeName", target, name)
    }

    async type(): Promise<void> {
        await this.connection?.send("Type")
    }

    async leave(): Promise<void> {
        await this.connection?.send("Leave")
        await this.connection?.stop()
    }
}

