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
    [Column("number")]
    public required string Number { get; set; }

    [NotMapped]
    public List<Balance> Balances { get; set; }
}
