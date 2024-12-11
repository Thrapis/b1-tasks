using Task_2.Models.Database.Entities;

namespace Task_2.TrialBalanceExcel.Entities;

public record class TrialBalanceFileData
{
    public string FileName { get; set; }
    public DateTime Created { get; set; }
    public string OrganisationName { get; set; }
    public DateOnly PeriodStart { get; set; }
    public DateOnly PeriodEnd { get; set; }
    public DateTime DataTime { get; set; }
    public string Currency { get; set; }

    public List<TrialBalanceClass> Classes { get; set; }

    public decimal OpeningBalanceActiveSum => Classes.Sum(a => a.OpeningBalanceActiveSum);
    public decimal OpeningBalancePassiveSum => Classes.Sum(a => a.OpeningBalancePassiveSum);
    public decimal TurnoverDebitSum => Classes.Sum(a => a.TurnoverDebitSum);
    public decimal TurnoverCreditSum => Classes.Sum(a => a.TurnoverCreditSum);
    public decimal ClosingBalanceActiveSum => Classes.Sum(a => a.ClosingBalanceActiveSum);
    public decimal ClosingBalancePassiveSum => Classes.Sum(a => a.ClosingBalancePassiveSum);
}
