import { AxiosPromise } from "axios"
import { apiInstance } from "../Base"
import { IUploadedFile } from "./Types"

export const UploadFile = (file: File): AxiosPromise => {
    let formData = new FormData()
    formData.append("file", file);
    return apiInstance.post('/TrialBalance/UploadFile', formData)
}

export const GetUploadedFiles = (): AxiosPromise<IUploadedFile[]> => {
    return apiInstance.get('/TrialBalance/GetUploadedFiles')
}