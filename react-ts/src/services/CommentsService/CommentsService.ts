import {AxiosInstance} from "axios";
import {ICommentsService} from "./ICommentsService.ts";
import {AddBody} from "./InputModels/AddBody.ts";
import {GetQuery} from "./InputModels/GetQuery.ts";
import {commentSchema, commentsSchema} from "./Validators/CommentsValidator.ts";
import {Comments, Comment} from "./Models/Comments.ts";


// Экспорт класса CommentsService для его использования из других модулей
export class CommentsService implements ICommentsService {

    // Сервис для отправки запросов по API
    private axiosInstance: AxiosInstance

    private readonly authUrl: string

    constructor(axiosInstance: AxiosInstance, authUrl: string) {
        this.axiosInstance = axiosInstance
        this.authUrl = authUrl
    }

    async get(query: GetQuery): Promise<Comments> {

        const response = await this.axiosInstance.get<Comments>('/comments', {params: query});

        await commentsSchema.validate(response.data)

        response.data.comments.forEach(c => {
            if (c.avatarUrl) c.avatarUrl = `${this.authUrl}/${c.avatarUrl}`
        })

        return response.data
    }

    async delete(id: string): Promise<void> {
        await this.axiosInstance.delete(`/comments/${id}`);
    }

    async add(body: AddBody): Promise<Comment> {

        const response = await this.axiosInstance.put<Comment>('/comments', body);

        await commentSchema.validate(response.data)

        if (response.data.avatarUrl) response.data.avatarUrl = `${this.authUrl}/${response.data.avatarUrl}`

        return response.data
    }
}

