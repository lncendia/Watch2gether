import {SyncEvent} from "ts-events";

export interface IPlayerHandler {
    pause: SyncEvent<[boolean, number]>
    seek: SyncEvent<number>
    fullscreen: SyncEvent<boolean>
    changeSeries: SyncEvent<[number, number]>

    unmount(): void

    setSecond(second: number): void

    setPause(pause: boolean): void
}