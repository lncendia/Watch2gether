import {SyncEvent} from "ts-events";
import {IPlayerHandler} from "./IPlayerHandler.ts";

export class PlayerJsHandler implements IPlayerHandler {

    private iframe: HTMLIFrameElement
    pause = new SyncEvent<[boolean, number]>();
    seek = new SyncEvent<number>();
    fullscreen = new SyncEvent<boolean>();
    changeSeries = new SyncEvent<[number, number]>();

    constructor(iframe: HTMLIFrameElement) {
        this.iframe = iframe;
        window.addEventListener("message", this.handler.bind(this))
    }

    private handler(event: MessageEvent<any>) {
        if ((event.data.event === 'play' || event.data.event === 'buffered') && event.data.time != null) {
            const time = parseInt(event.data.time);
            this.pause.post([false, time])
        } else if ((event.data.event === 'pause' || event.data.event === 'buffering') && event.data.time != null) {
            const time = parseInt(event.data.time);
            this.pause.post([true, time])
        } else if (event.data.event === 'seek' && event.data.time != null) {
            const time = parseInt(event.data.time);
            this.seek.post(time)
        } else if (event.data.event === 'new') {
            const data = event.data.id.split('_') as Array<string>
            if (data.length === 1) return
            const season = parseInt(data[0]);
            const series = parseInt(data[1]);
            this.changeSeries.post([season, series])
        } else if (event.data.event === 'fullscreen') {
            this.fullscreen.post(true)
        } else if (event.data.event === 'exitfullscreen') {
            this.fullscreen.post(false)
        }
    }

    setSecond(second: number): void {
        this.iframe.contentWindow!.postMessage({'api': 'seek', 'set': second}, '*');
    }

    setPause(pause: boolean): void {
        const message = pause ? 'pause' : 'play';
        this.iframe.contentWindow!.postMessage({'api': message}, '*');
    }
}