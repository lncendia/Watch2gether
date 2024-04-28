import {AxiosInstance} from "axios";
import {ICommentsService} from "./ICommentsService.ts";
import {List} from "../Common/Models/List.ts";
import {AddInputModel, GetInputModel} from "./InputModels/CommentInputModels.ts";
import {Comment} from "./Models/CommentViewModels.ts";


// Экспорт класса CommentsService для его использования из других модулей
export class CommentsService implements ICommentsService {

    // Сервис для отправки запросов по API
    private axiosInstance: AxiosInstance

    private readonly authUrl: string

    constructor(axiosInstance: AxiosInstance, authUrl: string) {
        this.axiosInstance = axiosInstance
        this.authUrl = authUrl
    }

    async get(query: GetInputModel): Promise<List<Comment>> {

        const response = await this.axiosInstance.get<List<Comment>>('/comments', {params: query});

        response.data.list.forEach(c => {
            if (c.avatarUrl) c.avatarUrl = `${this.authUrl}/${c.avatarUrl}`
        })

        return response.data
    }

    async delete(id: string): Promise<void> {
        await this.axiosInstance.delete(`/comments/${id}`);
    }

    async add(body: AddInputModel): Promise<Comment> {

        const response = await this.axiosInstance.put<Comment>('/comments', body);

        if (response.data.avatarUrl) response.data.avatarUrl = `${this.authUrl}/${response.data.avatarUrl}`

        return response.data
    }
}

