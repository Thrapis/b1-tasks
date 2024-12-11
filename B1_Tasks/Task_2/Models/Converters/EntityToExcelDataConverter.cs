using Task_2.Models.Database.Entities;
using Task_2.TrialBalanceExcel.Entities;

namespace Task_2.Models.Converters;

public static class EntityToExcelDataConverter
{

    public static TrialBalanceFileData FileDataFromExcelFile(ExcelFile excelFile)
    {
        var accountClasses = excelFile.Balances.GroupBy(b => b.Account!.Class)
            .Select(g => new TrialBalanceClass()
            {
                Name = g.Key.Name,
                Number = g.Key.Number,
                Balances = g.Select(b => new TrialBalanceRecord()
                {
                    Number = b.Account.Number,
                    OpeningBalanceActive = b.OpeningBalanceActive,
                    OpeningBalancePassive = b.OpeningBalancePassive,
                    TurnoverDebit = b.TurnoverDebit,
                    TurnoverCredit = b.TurnoverCredit,
                    ClosingBalanceActive = b.ClosingBalanceActive,
                    ClosingBalancePassive = b.ClosingBalancePassive,
                }).ToList()
            }).ToList();

        return new TrialBalanceFileData()
        {
            FileName = excelFile.Name,
            Created = excelFile.Uploaded,
            OrganisationName = excelFile.Organisation.Name,
            PeriodStart = excelFile.PeriodStart,
            PeriodEnd = excelFile.PeriodEnd,
            DataTime = excelFile.DataTime,
            Currency = excelFile.Currency.Symbol,
            Classes = accountClasses
        };
    }
}
