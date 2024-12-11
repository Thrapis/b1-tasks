import { IUploadedFile } from "shared/api/excel";

type Props = {
    onClick?: (id: number) => void
    uploadedFile: IUploadedFile
}

export const UploadedFileView = ({
    onClick,
    uploadedFile
}: Props) => {

    return(
        <button className="d-flex flex-column btn btn-light text-start" onClick={() => onClick?.(uploadedFile.id)}>
            <div>
                <label>
                    Name:
                    <span className="ps-2">{uploadedFile.name}</span>
                </label>
            </div>
            <div>
                <label>
                    Data time:
                    <span className="ps-2">{new Date(uploadedFile.dataTime).toLocaleString('de-DE', {timeZone: 'Europe/London'})}</span>
                </label>
            </div>
            <div>
                <label>
                    Uploaded:
                    <span className="ps-2">{new Date(uploadedFile.uploaded).toLocaleString('de-DE', {timeZone: 'Europe/London'})}</span>
                </label>
            </div>
        </button>
    )
}