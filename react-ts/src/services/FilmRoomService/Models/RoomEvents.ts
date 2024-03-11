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

export interface BeepEvent {
    userId: string,
    targetId: string
}

export interface ScreamEvent {
    userId: string,
    targetId: string
}

export interface KickEvent {
    userId: string,
    targetId: string
}

export interface ChangeNameEvent {
    userId: string,
    targetId: string,
    name: string
}

export interface ConnectEvent{

}