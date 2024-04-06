import {IProfileService} from "./IProfileService.ts";
import {AddRatingBody} from "./InputModels/AddRatingBody.ts";
import {GetRatingsQuery} from "./InputModels/GetRatingsQuery.ts";
import {Profile} from "./Models/Profile.ts";
import {Ratings} from "./Models/Ratings.ts";
import {AxiosInstance} from "axios";
import {profileSchema, ratingsSchema} from "./Validators/ProfileValidator.ts";

export class ProfileService implements IProfileService {

    // Сервис для отправки запросов по API
    private axiosInstance: AxiosInstance

    constructor(axiosInstance: AxiosInstance) {
        this.axiosInstance = axiosInstance;
    }

    async profile(): Promise<Profile> {
        const response = await this.axiosInstance.get<Profile>('profile/profile')

        // Валидация ответа сервера
        await profileSchema.validate(response.data);

        return response.data
    }

    async ratings(query: GetRatingsQuery): Promise<Ratings> {
        const response = await this.axiosInstance.get<Ratings>('profile/ratings', {params: query})

        // Валидация ответа сервера
        await ratingsSchema.validate(response.data);

        return response.data
    }

    async addRating(body: AddRatingBody): Promise<void> {
        await this.axiosInstance.put('profile/addRating', body)
    }

    async addToHistory(filmId: string): Promise<void> {
        await this.axiosInstance.put('profile/addToHistory', {filmId: filmId})
    }

    async toggleWatchlist(filmId: string): Promise<void> {
        await this.axiosInstance.post('profile/toggleWatchlist', {filmId: filmId})
    }

    async changeAllows(body: ChangeAllowsBody): Promise<void> {
        await this.axiosInstance.post('profile/changeAllows', body)
    }

}