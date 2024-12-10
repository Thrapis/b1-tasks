using Task_2.Models.Database.Entities;
using Task_2.Models.ViewModels;

namespace Task_2.Models.Converters;

public static class EntityToViewModelConverter
{
    public static IEnumerable<UploadedFile> ExcelFilesToUploadedFiles(IEnumerable<ExcelFile> excelFiles)
    {
        return excelFiles.Select(e => new UploadedFile()
        {
            Id = e.Id,
            Name = e.Name,
            DataTime = e.DataTime,
            Uploaded = e.Uploaded,
        }).ToList();
    }

    public static FileContent ExcelDataToFileContent(ExcelFile excelFile)
    {
        var groupedAccounts = excelFile.Balances!.GroupBy(b => b.Account!.Number.Substring(0, 2));

        return new FileContent()
        {
            FileId = excelFile.Id,
            PeriodStart = excelFile.PeriodStart,
            PeriodEnd = excelFile.PeriodEnd,
            DataTime = excelFile.DataTime,
            Uploaded = excelFile.Uploaded,
            OrganisationName = excelFile.Organisation?.Name ?? string.Empty,
            CurrencySymbol = excelFile.Currency?.Symbol ?? string.Empty,
            AccountGroups = groupedAccounts!.Select(ag => new AccountGroup()
            {
                GroupNumber = ag.Key,
                AccountBalances = ag!.Select(b => new AccountBalance()
                {
                    AccountId = b.Account!.Id,
                    AccountNumber = b.Account!.Number,
                    OpeningBalanceActive = b.OpeningBalanceActive,
                    OpeningBalancePassive = b.OpeningBalancePassive,
                    TurnoverDebit = b.TurnoverDebit,
                    TurnoverCredit = b.TurnoverCredit,
                    ClosingBalanceActive = b.ClosingBalanceActive,
                    ClosingBalancePassive = b.ClosingBalancePassive,
                }).ToList(),
            }).ToList(),
        };
    }
}
