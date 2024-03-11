import * as signalR from "@microsoft/signalr";
import {HubConnection} from "@microsoft/signalr";


export class FilmRoomService {
    private connection: HubConnection

    async connect(roomId: string) {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl("/chat")
            .configureLogging(signalR.LogLevel.Information)
            .build();

        this.connection.on('Error', (id, message) => room.ProcessReceiveEvent(new ErrorReceiveEvent(id, message)));
        this.connection.on('Message', (id, message) => room.ProcessReceiveEvent(new MessageReceiveEvent(id, message)));
        this.connection.on('Pause', (id, pause, second) => room.ProcessReceiveEvent(new PauseReceiveEvent(id, pause, second)));
        this.connection.on('Seek', (id, second) => room.ProcessReceiveEvent(new SeekReceiveEvent(id, second)));
        this.connection.on('ChangeSeries', (id, season, series) => room.ProcessReceiveEvent(new ChangeSeriesReceiveEvent(id, season, series)));
        this.connection.on('Leave', (id) => room.ProcessReceiveEvent(new LeaveReceiveEvent(id)));
        this.connection.on('Type', (id) => room.ProcessReceiveEvent(new TypeReceiveEvent(id)));
        this.connection.on('Beep', (id, target) => room.ProcessReceiveEvent(new BeepReceiveEvent(id, target)));
        this.connection.on('Scream', (id, target) => room.ProcessReceiveEvent(new ScreamReceiveEvent(id, target)));
        this.connection.on('Kick', (id, target) => room.ProcessReceiveEvent(new KickReceiveEvent(id, target)));
        this.connection.on('FullScreen', (id, fullscreen) => room.ProcessReceiveEvent(new FullScreenReceiveEvent(id, fullscreen)));
        this.connection.on('Connect', (data) => {
            room.ProcessReceiveEvent(new ConnectFilmReceiveEvent(data.id, data.username, data.avatar, data.time, data.beep, data.scream, data.change, data.season, data.series))
        });

        await this.connection.start();
    }
}

