using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task_2.Models.Database.Entities;

[Table("organisations")]
public class Organisation
{
    [Key, Column("id"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Column("name")]
    public required string Name { get; set; }
}
