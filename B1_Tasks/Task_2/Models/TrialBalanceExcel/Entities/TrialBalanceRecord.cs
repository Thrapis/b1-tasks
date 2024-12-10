namespace Task_2.TrialBalanceExcel.Entities;

public record class TrialBalanceRecord
{
    public string Number { get; set; }
    public decimal OpeningBalanceActive { get; set; }
    public decimal OpeningBalancePassive { get; set; }
    public decimal TurnoverDebit { get; set; }
    public decimal TurnoverCredit { get; set; }
    public decimal ClosingBalanceActive { get; set; }
    public decimal ClosingBalancePassive { get; set; }
}
