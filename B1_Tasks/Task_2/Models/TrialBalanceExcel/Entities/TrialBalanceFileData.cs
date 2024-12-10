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
}
