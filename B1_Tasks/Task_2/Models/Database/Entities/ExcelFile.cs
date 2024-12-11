using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task_2.Models.Database.Entities;

[Table("files")]
public class ExcelFile
{
    [Key, Column("id"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Column("currency_id")]
    public int CurrencyId { get; set; }
    [Column("organisation_id")]
    public int OrganisationId { get; set; }
    [Column("name")]
    public required string Name { get; set; }
    [Column("period_start")]
    public DateOnly PeriodStart { get; set; }
    [Column("period_end")]
    public DateOnly PeriodEnd { get; set; }
    [Column("data_time")]
    public DateTime DataTime { get; set; }
    [Column("uploaded")]
    public DateTime Uploaded { get; set; }


    public List<Balance> Balances { get; set; }
    public Currency? Currency { get; set; }
    public Organisation? Organisation { get; set; }
}