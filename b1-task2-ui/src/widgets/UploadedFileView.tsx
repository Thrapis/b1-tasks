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
        <button className="col btn btn-light text-start" onClick={() => onClick?.(uploadedFile.id)}>
            <div className="row">
                <label>
                    Name:
                    <span className="ps-2">{uploadedFile.name}</span>
                </label>
            </div>
            <div className="row">
                <label>
                    Data time:
                    <span className="ps-2">{new Date(uploadedFile.dataTime).toLocaleString()}</span>
                </label>
            </div>
            <div className="row">
                <label>
                    Uploaded:
                    <span className="ps-2">{new Date(uploadedFile.uploaded).toLocaleString()}</span>
                </label>
            </div>
        </button>
    )
}