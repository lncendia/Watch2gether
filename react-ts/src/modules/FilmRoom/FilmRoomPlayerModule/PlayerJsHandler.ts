import {IFilmRoomService} from "../../../services/FilmRoomService/IFilmRoomService.ts";
import {IPlayerHandler} from "./IPlayerHandler.ts";

export class PlayerJsHandler implements IPlayerHandler {
    constructor(service: IFilmRoomService) {
        this.service = service;
    }

    private service: IFilmRoomService

    async handler(event: MessageEvent<any>) {
        console.log(event.data)
        // let types = ['resumed', 'pause', 'paused', 'seek', 'play', 'buffered', 'buffering']
        // for (let i = 0; i < types.length; i++) {
        //     if (types[i] === event.data.event) console.log(event.data.event)
        // }
        if ((event.data.event === 'play' || event.data.event === 'buffered') && event.data.time != null) {
            let time = parseInt(event.data.time);
            await this.service.setPause(false, time)
        } else if ((event.data.event === 'pause' || event.data.event === 'buffering') && event.data.time != null) {
            let time = parseInt(event.data.time);
            await this.service.setPause(true, time)
        } else if (event.data.event === 'seek' && event.data.time != null) {
            let time = parseInt(event.data.time);
            await this.service.setTimeLine(time)
        } else if (event.data.event === 'new' && event.data.id != null) {
            let data = event.data.id.split('_')
            let season = parseInt(data[0]);
            let series = parseInt(data[1]);
            await this.service.changeSeries(season, series)
        } else if (event.data.event === 'fullscreen') {
            await this.service.setFullScreen(true)
        } else if (event.data.event === 'exitfullscreen') {
            await this.service.setFullScreen(false)
        }
    }
}