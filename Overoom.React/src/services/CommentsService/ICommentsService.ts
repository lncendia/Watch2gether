import {List} from "../Common/Models/List.ts";
import {AddInputModel, GetInputModel} from "./InputModels/CommentInputModels.ts";
import {Comment} from "./Models/CommentViewModels.ts";

export interface ICommentsService {
    get(query: GetInputModel): Promise<List<Comment>>

    delete(id: string): Promise<void>

    add(body: AddInputModel): Promise<Comment>
}