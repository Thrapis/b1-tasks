import axios from "axios"
import { ChangeEvent, FormEvent, useEffect, useState } from "react"
import { IUploadedFile, UploadFile, GetUploadedFiles } from "shared/api/excel"
import { UploadedFileView } from "widgets/UploadedFileView"


export const HomePage = () => {
    const [fileUploadLoading, setFileUploadLoading] = useState(false)
    const [filesLoading, setFilesLoading] = useState(true)
    const [uploadedFiles, setUploadedFiles] = useState<IUploadedFile[] | null>(null)
    const [selectedFile, setSelectedFile] = useState<File | null>(null)

    const fetchUploadedFiles = async () => {
        await GetUploadedFiles()
            .then(response => response.data)
            .then(data => {
                setUploadedFiles(data)
                setFilesLoading(false)
            })
            .catch(error => console.error('Error fetching data:', error))
    }

    const onFileClick = (id: number) => {
        console.log(id)
    }

    const handleUploadFileSubmit = async (event: FormEvent) => {
        event.preventDefault()
        if (selectedFile === null) {
            return
        }

        setFileUploadLoading(true)

        await UploadFile(selectedFile)
            .then(response => {
                if (response.status === axios.HttpStatusCode.Ok) {
                    setFileUploadLoading(false)
                    setFilesLoading(true)
                    fetchUploadedFiles()
                } else {
                    throw new Error("An error occurred while trying to upload the file")
                }
            })
            .catch(error => console.error('Error:', error))
    }

    const onFileInputChange = (event: ChangeEvent<HTMLInputElement>) => {
        setSelectedFile(event.target.files?.item(0) || null)
    }

    useEffect(() => {
        fetchUploadedFiles()
    }, [])

    return (
        <div className="container text-center min-vh-100 p-1 gap-4">

            <form className="row input-group mb-4" onSubmit={handleUploadFileSubmit}>
                <input type="file" className="form-control" onChange={onFileInputChange}/>
                <button type="submit" className="btn btn-success" >
                    Upload
                </button>
            </form>

            {
                fileUploadLoading && (<div className="row my-4">Loading...</div>)
            }

            <div className="row mt-4">
                <div className="col-3 border border-primary rounded py-2 px-1 gap-2">
                    {
                        filesLoading ? (<div>Loading...</div>) : (
                            uploadedFiles?.map(f => (
                                <UploadedFileView
                                    key={crypto.randomUUID()}
                                    uploadedFile={f}
                                    onClick={onFileClick}
                                />
                            ))
                        )
                    }
                </div>
                <div className="col-9 h-100">
                    Z
                </div>
            </div>
        </div>
    )
}