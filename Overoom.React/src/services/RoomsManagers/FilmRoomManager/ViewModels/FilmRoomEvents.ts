export interface PauseEvent {
    userId: string,
    onPause: boolean,
    seconds: number
}

export interface SeekEvent {
    userId: string,
    seconds: number,
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

export interface ActionEvent {
    initiator: string,
    target: string
}

export interface ChangeNameEvent extends ActionEvent {
    name: string
}

export interface ConnectEvent {

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