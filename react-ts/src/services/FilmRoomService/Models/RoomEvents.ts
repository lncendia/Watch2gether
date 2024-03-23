export interface NewMessageEvent {
    userId: string,
    text: string
}

export interface PauseMessageEvent {
    userId: string,
    onPause: boolean
}

export interface SeekEvent {
    userId: string,
    second: number,
}

export interface FullScreenEvent {
    userId: string,
    isFullScreen: boolean
}

export interface ChangeSeriesEvent {
    userId: string,
    season: number,
    series: number
}

export interface LeaveEvent {
    userId: string
}

export interface TypeEvent {
    userId: string
}

export interface ActionEvent {
    initiator: string,
    target: string
}

export interface ChangeNameEvent extends ActionEvent{
    name: string
}

export interface ConnectEvent{

}

export interface MessageEvent {
    id: string;
    userId: string;
    createdAt: Date;
    text: string;
}

export interface MessagesEvent {
    messages: MessageEvent[];
    count: number;
}