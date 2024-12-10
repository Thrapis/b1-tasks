namespace Task_2.TrialBalanceExcel.Entities;

public record class TrialBalanceClass
{
    public string Name { get; set; }
    public string Number { get; set; }
    public List<TrialBalanceRecord> Balances { get; set; }

    public IEnumerable<IGrouping<string, TrialBalanceRecord>> GetGroupedBalanceRecords()
    {
        return Balances.GroupBy(b => b.Number.Substring(0, 2));
    }

    public decimal OpeningBalanceActiveSum => Balances.Sum(a => a.OpeningBalanceActive);
    public decimal OpeningBalancePassiveSum => Balances.Sum(a => a.OpeningBalancePassive);
    public decimal TurnoverDebitSum => Balances.Sum(a => a.TurnoverDebit);
    public decimal TurnoverCreditSum => Balances.Sum(a => a.TurnoverCredit);
    public decimal ClosingBalanceActiveSum => Balances.Sum(a => a.ClosingBalanceActive);
    public decimal ClosingBalancePassiveSum => Balances.Sum(a => a.ClosingBalancePassive);
}
