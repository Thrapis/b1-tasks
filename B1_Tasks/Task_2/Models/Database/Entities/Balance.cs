using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task_2.Models.Database.Entities;

[Table("balances")]
public class Balance
{
    [Key, Column("id"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Column("file_id")]
    public int FileId { get; set; }
    [Column("account_id")]
    public int AccountId { get; set; }
    [Column("opening_balance_active")]
    public decimal OpeningBalanceActive { get; set; }
    [Column("opening_balance_passive")]
    public decimal OpeningBalancePassive { get; set; }
    [Column("turnover_debit")]
    public decimal TurnoverDebit { get; set; }
    [Column("turnover_credit")]
    public decimal TurnoverCredit { get; set; }
    [Column("closing_balance_active")]
    public decimal ClosingBalanceActive { get; set; }
    [Column("closing_balance_passive")]
    public decimal ClosingBalancePassive { get; set; }

    
    public ExcelFile? File { get; set; }
    public Account? Account { get; set; }
}
