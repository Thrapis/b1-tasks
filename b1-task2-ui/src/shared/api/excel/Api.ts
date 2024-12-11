import { AxiosPromise } from "axios"
import { apiInstance } from "../Base"
import { IFileContent, IUploadedFile } from "./Types"

// Upload file
export const UploadFile = (file: File): AxiosPromise => {
    let formData = new FormData()
    formData.append("file", file)
    return apiInstance.post('/TrialBalance/UploadFile', formData)
}

// Get list of uploaded files
export const GetUploadedFiles = (): AxiosPromise<IUploadedFile[]> => {
    return apiInstance.get('/TrialBalance/GetUploadedFiles')
}

// Get file view (table)
export const GetFileView = (fileId: number): AxiosPromise<IFileContent> => {
    return apiInstance.get(`/TrialBalance/GetFileView/${fileId}`)
}

// Download file
export const DownloadFileData = (form: FormData): AxiosPromise => {
    return apiInstance.post('/TrialBalance/DownloadFileData', form, { responseType: 'blob' })
}