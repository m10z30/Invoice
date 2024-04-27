using Microsoft.EntityFrameworkCore;
using Invoice.Models;

namespace Invoice.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DbContext> options) : base(options)
    {
        
    }


    public DbSet<InvoiceModel> Invoices { set; get; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InvoiceModel>()
            .HasIndex(i => new { i.InvoiceId, i.Id })
            .IsUnique(true);
    }

}
