using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task_2.Models.Database.Entities;

[Table("currencies")]
public class Currency
{
    [Key, Column("id"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Column("code_name")]
    public required string CodeName { get; set; }
    [Column("short_name")]
    public required string ShortName { get; set; }
    [Column("long_name")]
    public required string LongName { get; set; }
    [Column("symbol")]
    public required string Symbol { get; set; }
}
