import {CreateYoutubeRoomInputModel} from "./InputModels/YoutubeRoomsInputModels.ts";
import {ConnectRoomInputModel, RoomSearchInputModel} from "../Common/InputModels/RoomsInputModels.ts";
import {YoutubeRoom, YoutubeRoomShort} from "./ViewModels/YoutubeRoomsViewModels.ts";
import {List} from "../../Common/Models/List.ts";
import {RoomServer} from "../Common/ViewModels/RoomsViewModels.ts";

export interface IYoutubeRoomsService {

    create(body: CreateYoutubeRoomInputModel): Promise<RoomServer>

    connect(body: ConnectRoomInputModel): Promise<RoomServer>

    search(query: RoomSearchInputModel): Promise<List<YoutubeRoomShort>>

    my(): Promise<YoutubeRoomShort[]>

    room(id: string): Promise<YoutubeRoom>
}