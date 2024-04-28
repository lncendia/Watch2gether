import {Room} from "../../Common/ViewModels/RoomsViewModels.ts";

export interface YoutubeRoomShort extends Room {
    videoAccess: boolean;
}

export interface YoutubeRoom extends YoutubeRoomShort {
    videoAccess: boolean;
    isCodeNeeded: boolean;
}