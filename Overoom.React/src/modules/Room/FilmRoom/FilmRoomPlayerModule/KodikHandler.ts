import {IFilmRoomService} from "../../../../services/FilmRoomService/IFilmRoomService.ts";
import {IPlayerHandler} from "./IPlayerHandler.ts";

export class KodikHandler implements IPlayerHandler {

    constructor(service: IFilmRoomService) {
        this.service = service;
    }

    private service: IFilmRoomService
    private time = 0
    private isTimeUpdateNeeded = true

    async handler(event: MessageEvent<any>): Promise<void> {
        if (event.data.title === "mousemove") return

        console.log(event.data)

        if (event.data.key === "kodik_player_time_update") {
            this.time = event.data.value

            if (this.isTimeUpdateNeeded) {
                await this.service.setTimeLine(this.time)
                this.isTimeUpdateNeeded = false
            }
        } else if (event.data.key === 'kodik_player_play') {
            await this.service.setPause(false, this.time)

        } else if (event.data.key === 'kodik_player_pause') {
            await this.service.setPause(true, this.time)
        } else if (event.data.key === 'kodik_player_seek') {
            await this.service.setTimeLine(event.data.value.time)
            this.isTimeUpdateNeeded = true

        } else if (event.data.key === 'kodik_player_current_episode') {
            await this.service.changeSeries(event.data.value.season, event.data.value.episode)
            this.isTimeUpdateNeeded = true
        }
    }
}