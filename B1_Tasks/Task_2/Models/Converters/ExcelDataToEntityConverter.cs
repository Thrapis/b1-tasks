using Task_2.Models.Database.Entities;
using Task_2.TrialBalanceExcel.Entities;

namespace Task_2.Models.Converters;

public static class ExcelDataToEntityConverter
{
    public static ExcelFile GetFileFromData(TrialBalanceFileData data)
    {
        return new ExcelFile()
        {
            Name = data.FileName,
            PeriodStart = data.PeriodStart,
            PeriodEnd = data.PeriodEnd,
            Uploaded = DateTime.SpecifyKind(data.Created, DateTimeKind.Utc),
            DataTime = DateTime.SpecifyKind(data.DataTime, DateTimeKind.Utc),
        };
    }

    public static Organisation GetOrganisationFromData(TrialBalanceFileData data)
    {
        return new Organisation()
        {
            Name = data.OrganisationName,
        };
    }

    public static Currency GetCurrencyFromData(TrialBalanceFileData data)
    {
        return new Currency()
        {
            CodeName = string.Empty,
            ShortName = string.Empty,
            LongName = string.Empty,
            Symbol = data.Currency,
        };
    }

    public static IEnumerable<AccountClass> GetClassAndNestedFromData(TrialBalanceFileData data)
    {
        return data.Classes.Select(c => new AccountClass() {
            Name = c.Name,
            Number = c.Number,
            Accounts = c.Balances.Select(b => new Account()
            {
                Number = b.Number,
                Balances = new List<Balance>()
                {
                    new()
                    {
                        OpeningBalanceActive = b.OpeningBalanceActive,
                        OpeningBalancePassive = b.OpeningBalancePassive,
                        TurnoverDebit = b.TurnoverDebit,
                        TurnoverCredit = b.TurnoverCredit,
                        ClosingBalanceActive = b.ClosingBalanceActive,
                        ClosingBalancePassive = b.ClosingBalancePassive,
                    }
                }
            }).ToList(),
        });
    }
}
