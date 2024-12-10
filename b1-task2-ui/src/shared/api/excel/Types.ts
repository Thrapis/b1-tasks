
// Uploaded file data

export interface IUploadedFile {
    readonly id: number
    readonly name: string
    readonly uploaded: string
    readonly dataTime: string
}

// File view data

export interface IFileContent {
    fileId: number
    organisationName: string
    periodStart: string
    periodEnd: string
    dataTime: string
    uploaded: string
    currencySymbol: string
    accountGroups: IAccountGroup[]
    openingBalanceActiveOverall: number
    openingBalancePassiveOverall: number
    turnoverDebitOverall: number
    turnoverCreditOverall: number
    closingBalanceActiveOverall: number
    closingBalancePassiveOverall: number
}

export interface IAccountGroup {
    groupNumber: string
    accountBalances: IAccountBalance[]

    openingBalanceActiveGroupOverall: number
    openingBalancePassiveGroupOverall: number
    turnoverDebitGroupOverall: number
    turnoverCreditGroupOverall: number
    closingBalanceActiveGroupOverall: number
    closingBalancePassiveGroupOverall: number
}

export interface IAccountBalance {
    accountId: number
    accountNumber: string
    openingBalanceActive: number
    openingBalancePassive: number
    turnoverDebit: number
    turnoverCredit: number
    closingBalanceActive: number
    closingBalancePassive: number
}