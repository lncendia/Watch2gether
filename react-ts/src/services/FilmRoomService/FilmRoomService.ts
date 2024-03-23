import * as signalR from "@microsoft/signalr";
import {HubConnection} from "@microsoft/signalr";
import {IFilmRoomService} from "./IFilmRoomService.ts";
import {SyncEvent} from "ts-events";
import filmRoomSchema from "./Validators/RoomValidator.ts";
import {actionSchema, messageSchema, messagesSchema} from "./Validators/EventsValidator.ts";
import {ActionEvent, MessageEvent, MessagesEvent} from "./Models/RoomEvents.ts";


export class FilmRoomService implements IFilmRoomService {

    private connection?: HubConnection
    private tokenFactory: () => Promise<string>

    private readonly authUrl: string

    roomLoadEvent = new SyncEvent<FilmRoom>();

    messagesEvent = new SyncEvent<MessagesEvent>();

    messageEvent = new SyncEvent<MessageEvent>();

    beepEvent = new SyncEvent<ActionEvent>()

    screamEvent = new SyncEvent<ActionEvent>()

    constructor(tokenFactory: () => Promise<string>, authUrl: string) {
        this.authUrl = authUrl
        this.tokenFactory = tokenFactory
    }

    async connect(roomId: string, url: string) {

        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(url + "filmRoom", {accessTokenFactory: this.tokenFactory})
            .configureLogging(signalR.LogLevel.Information)
            .build();

        this.connection.on("Room", async (room: FilmRoom) => {
            room.viewers.forEach(v => {
                if (v.photoUrl) v.photoUrl = `${this.authUrl}/${v.photoUrl}`
            })
            await filmRoomSchema.validate(room)
            this.roomLoadEvent.post(room)
        })

        this.connection.on("Messages", async (messages: MessagesEvent) => {
            messages.messages.forEach(m => {
                m.createdAt = new Date(m.createdAt)
            })
            await messagesSchema.validate(messages)
            this.messagesEvent.post(messages)
        })

        this.connection.on('NewMessage', async (message: MessageEvent) => {
            message.createdAt = new Date(message.createdAt)
            await messageSchema.validate(message)
            this.messageEvent.post(message)
        });

        this.connection.on('Beep', async (action: ActionEvent) => {
            await actionSchema.validate(action)
            this.beepEvent.post(action)
        });

        this.connection.on('Scream', async (action: ActionEvent) => {
            await actionSchema.validate(action)
            this.screamEvent.post(action)
        });
        // this.connection.on('Error', (id, message) => room.ProcessReceiveEvent(new ErrorReceiveEvent(id, message)));
        // this.connection.on('Pause', (id, pause, second) => room.ProcessReceiveEvent(new PauseReceiveEvent(id, pause, second)));
        // this.connection.on('Seek', (id, second) => room.ProcessReceiveEvent(new SeekReceiveEvent(id, second)));
        // this.connection.on('ChangeSeries', (id, season, series) => room.ProcessReceiveEvent(new ChangeSeriesReceiveEvent(id, season, series)));
        // this.connection.on('Leave', (id) => room.ProcessReceiveEvent(new LeaveReceiveEvent(id)));
        // this.connection.on('Type', (id) => room.ProcessReceiveEvent(new TypeReceiveEvent(id)));
        // this.connection.on('Beep', (id, target) => room.ProcessReceiveEvent(new BeepReceiveEvent(id, target)));
        // this.connection.on('Scream', (id, target) => room.ProcessReceiveEvent(new ScreamReceiveEvent(id, target)));
        // this.connection.on('Kick', (id, target) => room.ProcessReceiveEvent(new KickReceiveEvent(id, target)));
        // this.connection.on('FullScreen', (id, fullscreen) => room.ProcessReceiveEvent(new FullScreenReceiveEvent(id, fullscreen)));
        // this.connection.on('Connect', (data) => {
        //     room.ProcessReceiveEvent(new ConnectFilmReceiveEvent(data.id, data.username, data.avatar, data.time, data.beep, data.scream, data.change, data.season, data.series))
        // });

        await this.connection.start();
        await this.connection.send("Connect", roomId)
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
}

