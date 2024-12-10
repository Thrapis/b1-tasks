using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task_2.Models.Database.Entities;


[Table("accounts")]
public class Account
{
    [Key, Column("id"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Column("class_id")]
    public int ClassId { get; set; }
    [Column("currency_id")]
    public int CurrencyId { get; set; }
    [Column("organisation_id")]
    public int OrganisationId { get; set; }
    [Column("number")]
    public required string Number { get; set; }

    [NotMapped]
    public IEnumerable<Balance> Balances { get; set; }
}
