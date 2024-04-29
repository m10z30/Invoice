using Microsoft.EntityFrameworkCore;
using Invoice.Models;
using Microsoft.EntityFrameworkCore.Design;

namespace Invoice.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DbContext> options) : base(options)
    {

    }

    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<InvoiceModel> Invoices { set; get; }
    public DbSet<MetaData> MetaDatas { set; get; }

}


public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{

    private readonly IConfiguration _configuration;

    public DataContextFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DataContextFactory() : base()
    {

    }

    public DataContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseNpgsql("User ID=test;Password=test;Server=localhost;Port=5432;Database=invoice_db;Pooling=true;");

        return new DataContext(optionsBuilder.Options);
    }
}


