using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task_2.Models.Database.Entities;

[Table("files")]
public class File
{
    [Key, Column("id"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Column("name")]
    public required string Name { get; set; }
    [Column("data_time")]
    public DateTime DataTime { get; set; }
    [Column("uploaded")]
    public DateTime Uploaded { get; set; }
}
