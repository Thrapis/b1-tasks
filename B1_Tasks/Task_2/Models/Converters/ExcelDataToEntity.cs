using Task_2.TrialBalanceExcel.Entities;

namespace Task_2.Models.Converters;

public class ExcelDataToEntity
{

    public static Database.Entities.File GetFileFromData(TrialBalanceFileData data)
    {
        
        return new Database.Entities.File()
        {
            Name = data.FileName,
            Uploaded = DateTime.SpecifyKind(data.Created, DateTimeKind.Utc),
            DataTime = DateTime.SpecifyKind(data.DataTime, DateTimeKind.Utc),
        };
    }

    public static Database.Entities.Organisation GetOrganisationFromData(TrialBalanceFileData data)
    {
        return new Database.Entities.Organisation()
        {
            Name = data.OrganisationName,
        };
    }

    public static Database.Entities.Currency GetCurrencyFromData(TrialBalanceFileData data)
    {
        return new Database.Entities.Currency()
        {
            CodeName = string.Empty,
            ShortName = string.Empty,
            LongName = string.Empty,
            Symbol = data.Currency,
        };
    }

    public static IEnumerable<Database.Entities.AccountClass> GetClassAndNestedFromData(TrialBalanceFileData data)
    {
        return data.Classes.Select(c => new Database.Entities.AccountClass() {
            Name = c.Name,
            Number = c.Number,
            Accounts = c.Balances.Select(b => new Database.Entities.Account()
            {
                Number = b.Number,
                Balances = new List<Database.Entities.Balance>()
                {
                    new()
                    {
                        PeriodStart = data.PeriodStart,
                        PeriodEnd = data.PeriodEnd,
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
