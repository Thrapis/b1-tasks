import axios from "axios"
import { ChangeEvent, FormEvent, useEffect, useState } from "react"
import { IUploadedFile, IFileContent, UploadFile, GetUploadedFiles } from "shared/api/excel"
import { DownloadFileData, GetFileView } from "shared/api/excel/Api"
import { FileView } from "widgets/FileView"
import { UploadedFileView } from "widgets/UploadedFileView"

import Modal from 'react-bootstrap/Modal';
import { Button } from "react-bootstrap"


export const HomePage = () => {
    // File uploading
    const [fileUploadLoading, setFileUploadLoading] = useState(false)
    const [selectedFile, setSelectedFile] = useState<File | null>(null)

    // File list
    const [filesLoading, setFilesLoading] = useState(true)
    const [uploadedFiles, setUploadedFiles] = useState<IUploadedFile[] | null>(null)

    // Table view
    const [viewLoading, setViewLoading] = useState(false)
    const [selectedFileView, setSelectedFileView] = useState<IFileContent | null>(null)

    // Modal
    const [showModal, setShowModal] = useState(false);
    const handleCloseModal = () => setShowModal(false);
    const handleShowModal = () => selectedFileView && setShowModal(true);

    // Get list of files
    const fetchUploadedFiles = async () => {
        await GetUploadedFiles()
            .then(response => response.data)
            .then(data => {
                setUploadedFiles(data)
                setFilesLoading(false)
            })
            .catch(error => console.error('Error fetching data:', error))
    }

    // Get table of clicked file
    const onFileClick = async (id: number) => {
        setViewLoading(true)

        await GetFileView(id)
            .then(response => response.data)
            .then(data => {
                setSelectedFileView(data)
                setViewLoading(false)
            })
            .catch(error => console.error('Error fetching data:', error))
    }

    // Submit upload file
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
                }
            })
            .catch(error => console.error('Error:', error))
    }

    // Set file to upload
    const onFileInputChange = (event: ChangeEvent<HTMLInputElement>) => {
        setSelectedFile(event.target.files?.item(0) || null)
    }

    // Download xlsx file (with settings)
    const handleDownloadFileDataSubmit = async (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault()
        if (selectedFileView === null) {
            return
        }

        const formData = new FormData(event.currentTarget)

        await DownloadFileData(formData)
            .then(response => {
                const xlsxBlob = new Blob([response.data], { type: 'application/xlsx' })
                const url = window.URL.createObjectURL(xlsxBlob)

                let name = response.headers['content-disposition'].split('filename=')[1].split('.')[0];
                let extension = response.headers['content-disposition'].split('.')[1].split(';')[0];
                const filename = `${name}.${extension}`

                const tempLink = document.createElement('a')
                tempLink.href = url
                tempLink.setAttribute("download", filename)

                document.body.appendChild(tempLink)
                tempLink.click()

                document.body.removeChild(tempLink)
                window.URL.revokeObjectURL(url)
            })
            .catch(error => console.error('Error downloading Excel file:', error))
    }

    // String formatter (utils)
    const substringIfLonger = (str: string, length: number) => {
        return str.length > length ?
            str.substring(0, length - 3).trim() + "..." :
            str
    }

    // Load files on start
    useEffect(() => {
        fetchUploadedFiles()
    }, [])

    return (
        <div className="container text-center min-vh-100 p-1 gap-4">

            <form className="d-flex flex-row input-group mb-4" onSubmit={handleUploadFileSubmit}>
                <input type="file" className="form-control" onChange={onFileInputChange} />
                <button type="submit" className="btn btn-success" >
                    Upload
                </button>
            </form>

            {
                fileUploadLoading && (<div className="row my-4">Loading...</div>)
            }

            <div className="d-flex justify-content-between mt-4 gap-4">
                <div className="d-flex overflow-auto border border-primary rounded p-2 gap-2">
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

                <Button variant="info" onClick={handleShowModal}>
                    Download file data
                </Button>

                {/* Modal */}
                <Modal show={showModal} onHide={handleCloseModal}>
                    <Modal.Header closeButton>
                        <Modal.Title>Download file data</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <form onSubmit={handleDownloadFileDataSubmit}>
                            <input type="hidden" name="fileId" value={selectedFileView?.fileId} />

                            <div className="d-flex flex-column gap-2 my-3 overflow-auto" style={{ maxHeight: "480px" }} >
                                {selectedFileView?.accountGroupClasses.map(c => (
                                    <label className="d-flex flex-row justify-content-between btn btn-light" key={crypto.randomUUID()}>
                                        {substringIfLonger(c.className, 48)}
                                        <input type="checkbox" name="classIds" value={c.classNumber} defaultChecked={true} />
                                    </label>
                                ))}
                            </div>

                            <button className="btn btn-primary" type="submit">
                                Download
                            </button>
                        </form>
                    </Modal.Body>
                </Modal>
            </div>

            <div className="row mt-4">
                <div className="col-12 h-100 border border-warning overflow-auto" style={{ maxHeight: 560 }}>
                    {
                        viewLoading ? (<div className="row my-4">Loading...</div>) : (
                            selectedFileView && <FileView file={selectedFileView} />
                        )
                    }
                </div>
            </div>
        </div>
    )
}