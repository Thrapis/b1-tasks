import axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse } from 'axios'
import { API_SOURCE } from 'shared/config'

class ApiInstance {
    private axios: AxiosInstance

    constructor() {
        this.axios = axios.create({
            baseURL: API_SOURCE,
        })
    }

    async get<T>(
        endpoint: string,
        options?: AxiosRequestConfig
    ): Promise<AxiosResponse<T>> {
        const response: AxiosResponse<T> = await this.axios.get(
            endpoint,
            options
        )
        return response
    }

    async post<T>(
        endpoint: string,
        data?: any,
        options?: AxiosRequestConfig
    ): Promise<AxiosResponse<T>> {
        const response: AxiosResponse<T> = await this.axios.post(
            endpoint,
            data,
            options
        )
        return response
    }
}

export const apiInstance = new ApiInstance()