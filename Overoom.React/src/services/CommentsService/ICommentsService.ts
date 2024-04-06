import {GetQuery} from "./InputModels/GetQuery.ts";
import {AddBody} from "./InputModels/AddBody.ts";
import {Comment, Comments} from "./Models/Comments.ts";

export interface ICommentsService {
    get(query: GetQuery): Promise<Comments>

    delete(id: string): Promise<void>

    add(body: AddBody): Promise<Comment>
}