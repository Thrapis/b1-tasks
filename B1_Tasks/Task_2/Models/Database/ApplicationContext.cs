using Microsoft.EntityFrameworkCore;

namespace Task_2.Models.Database;

public class ApplicationContext : DbContext
{
    public required DbSet<Entities.Currency> Currencies { get; set; }
    public required DbSet<Entities.Organisation> Organisations { get; set; }
    public required DbSet<Entities.AccountClass> AccountClasses { get; set; }
    public required DbSet<Entities.Account> Accounts { get; set; }
    public required DbSet<Entities.ExcelFile> Files { get; set; }
    public required DbSet<Entities.Balance> Balances { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        //Database.EnsureCreated();   // создаем базу данных при первом обращении
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}
