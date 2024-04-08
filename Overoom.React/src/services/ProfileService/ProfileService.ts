import {IProfileService} from "./IProfileService.ts";
import {AxiosInstance} from "axios";
import {List} from "../Common/Models/List.ts";
import {Profile, Rating} from "./ViewModels/ProfileViewModels.ts";
import {AddRatingInputModel, ChangeAllowsInputModel, GetRatingsInputModel} from "./InputModels/ProfileInputModels.ts";


export class ProfileService implements IProfileService {

    // Сервис для отправки запросов по API
    private axiosInstance: AxiosInstance

    constructor(axiosInstance: AxiosInstance) {
        this.axiosInstance = axiosInstance;
    }

    async profile(): Promise<Profile> {
        const response = await this.axiosInstance.get<Profile>('profile/profile')

        return response.data
    }

    async ratings(query: GetRatingsInputModel): Promise<List<Rating>> {
        const response = await this.axiosInstance.get<List<Rating>>('profile/ratings', {params: query})

        return response.data
    }

    async addRating(body: AddRatingInputModel): Promise<void> {
        await this.axiosInstance.put('profile/addRating', body)
    }

    async addToHistory(filmId: string): Promise<void> {
        await this.axiosInstance.put('profile/addToHistory', {filmId: filmId})
    }

    async toggleWatchlist(filmId: string): Promise<void> {
        await this.axiosInstance.post('profile/toggleWatchlist', {filmId: filmId})
    }

    async changeAllows(body: ChangeAllowsInputModel): Promise<void> {
        await this.axiosInstance.post('profile/changeAllows', body)
    }

}