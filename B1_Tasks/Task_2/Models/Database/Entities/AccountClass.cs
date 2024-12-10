using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task_2.Models.Database.Entities;


[Table("classes")]
public class AccountClass
{
    [Key, Column("id"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Column("number")]
    public required string Number { get; set; }
    [Column("name")]
    public required string Name { get; set; }

    [NotMapped]
    public IEnumerable<Account> Accounts { get; set; }
}
