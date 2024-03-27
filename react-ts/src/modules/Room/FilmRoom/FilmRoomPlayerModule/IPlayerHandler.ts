import {SyncEvent} from "ts-events";

export interface IPlayerHandler {
    pause: SyncEvent<[boolean, number]>
    seek: SyncEvent<number>
    fullscreen: SyncEvent<boolean>
    changeSeries: SyncEvent<[number, number]>

    mount(iframe: HTMLIFrameElement): void

    unmount(): void

    setSecond(second: number): void

    setPause(pause: boolean): void

    setSeries(season: number, episode: number): void

    generateUrl(base: string, second: number, season?: number, episode?: number): string
}