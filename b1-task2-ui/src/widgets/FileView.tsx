import { IAccountBalance, IAccountGroup, IAccountGroupClass, IFileContent } from "shared/api/excel"

const formatNumber = (num: number) => {
    return num.toLocaleString([], {
        maximumFractionDigits: 2,
        useGrouping: true,
    })
}

const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('ru-RU', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
    })
}

const formatDateTime = (dateTimeString: string) => {
    return new Date(dateTimeString)
        .toLocaleString('de-DE', {timeZone: 'Europe/London'})
        .replaceAll(',', ' ')
}

type FileViewProps = {
    file: IFileContent
}

const fileOverallTitle = "БАЛАНС"

export const FileView = ({
    file
}: FileViewProps) => {

    return (
        <table className="table table-bordered table-hover table-sm">
            <tbody>
                <FileMetaView file={file} />
                {file.accountGroupClasses.map(c => (
                    <FileAccountClassView
                        key={crypto.randomUUID()}
                        groupClass={c}
                    />
                ))}
                <tr>
                    <th className="text-start">{fileOverallTitle}</th>
                    <th className="text-end">{formatNumber(file.openingBalanceActiveOverall)}</th>
                    <th className="text-end">{formatNumber(file.openingBalancePassiveOverall)}</th>
                    <th className="text-end">{formatNumber(file.turnoverDebitOverall)}</th>
                    <th className="text-end">{formatNumber(file.turnoverCreditOverall)}</th>
                    <th className="text-end">{formatNumber(file.closingBalanceActiveOverall)}</th>
                    <th className="text-end">{formatNumber(file.closingBalancePassiveOverall)}</th>
                </tr>
            </tbody>
        </table>
    )
}

type FileMetaViewProps = {
    file: IFileContent
}

const titleLine1 = "Оборотная ведомость по балансовым счетам"
const titleLine3 = "по банку"

export const FileMetaView = ({
    file
}: FileMetaViewProps) => {

    return (
        <>
            <tr>
                <td className="text-start">{file.organisationName}</td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td colSpan={7}><h4>{titleLine1}</h4></td>
            </tr>
            <tr>
                <th colSpan={7}>{`за период с ${formatDate(file.periodStart)} по ${formatDate(file.periodEnd)}`}</th>
            </tr>
            <tr>
                <th colSpan={7}>{titleLine3}</th>
            </tr>
            <tr>
                <td colSpan={7} className="user-select-none" style={{color: 'transparent'}}>INVISIBLE</td>
            </tr>
            <tr>
                <td colSpan={2} className="text-start">{formatDateTime(file.dataTime)}</td>
                <td> </td>
                <td> </td>
                <td> </td>
                <td> </td>
                <td className="text-end">{`в ${file.currencySymbol}`}</td>
            </tr>
        </>
    )
}

type FileAccountClassProps = {
    groupClass: IAccountGroupClass
}

const classOverallTitle = "ПО КЛАССУ"

export const FileAccountClassView = ({
    groupClass
}: FileAccountClassProps) => {

    return (
        <>
            <tr>
                <th colSpan={7}>{groupClass.className}</th>
            </tr>
            {groupClass.accountGroups.map(g => (
                <FileAccountGroupView
                    key={crypto.randomUUID()}
                    group={g}
                />
            ))}
            <tr>
                <th className="text-start">{classOverallTitle}</th>
                <th className="text-end">{formatNumber(groupClass.openingBalanceActiveOverall)}</th>
                <th className="text-end">{formatNumber(groupClass.openingBalancePassiveOverall)}</th>
                <th className="text-end">{formatNumber(groupClass.turnoverDebitOverall)}</th>
                <th className="text-end">{formatNumber(groupClass.turnoverCreditOverall)}</th>
                <th className="text-end">{formatNumber(groupClass.closingBalanceActiveOverall)}</th>
                <th className="text-end">{formatNumber(groupClass.closingBalancePassiveOverall)}</th>
            </tr>
        </>
    )
}

type FileAccountGroupProps = {
    group: IAccountGroup
}

export const FileAccountGroupView = ({
    group
}: FileAccountGroupProps) => {

    return (
        <>
            {group.accountBalances.map(ab => (
                <FileAccountBalanceView
                    key={crypto.randomUUID()}
                    accountBalance={ab}
                />
            ))}
            <tr>
                <th className="text-start">{group.groupNumber}</th>
                <th className="text-end">{formatNumber(group.openingBalanceActiveOverall)}</th>
                <th className="text-end">{formatNumber(group.openingBalancePassiveOverall)}</th>
                <th className="text-end">{formatNumber(group.turnoverDebitOverall)}</th>
                <th className="text-end">{formatNumber(group.turnoverCreditOverall)}</th>
                <th className="text-end">{formatNumber(group.closingBalanceActiveOverall)}</th>
                <th className="text-end">{formatNumber(group.closingBalancePassiveOverall)}</th>
            </tr>
        </>
    )
}

type FileAccountBalanceProps = {
    accountBalance: IAccountBalance
}

export const FileAccountBalanceView = ({
    accountBalance
}: FileAccountBalanceProps) => {

    return (
        <tr>
            <td className="text-start">{accountBalance.accountNumber}</td>
            <td className="text-end">{formatNumber(accountBalance.openingBalanceActive)}</td>
            <td className="text-end">{formatNumber(accountBalance.openingBalancePassive)}</td>
            <td className="text-end">{formatNumber(accountBalance.turnoverDebit)}</td>
            <td className="text-end">{formatNumber(accountBalance.turnoverCredit)}</td>
            <td className="text-end">{formatNumber(accountBalance.closingBalanceActive)}</td>
            <td className="text-end">{formatNumber(accountBalance.closingBalancePassive)}</td>
        </tr>
    )
}