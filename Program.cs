using Invoice.Data;
using Invoice.Extensions;
using Invoice.Models;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        builder.Services.AddDbContext<DataContext>(
            opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreConnection"))
        );

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        app.UseLoggingMiddleware();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            var firstInvoice = await context.Invoices.Select(i => i.Id == 1).FirstOrDefaultAsync();
            if (!firstInvoice){
                var invoice = new InvoiceModel{
                  Id = 1,
                  InvoiceId = 1000,
                  CustomerName = "--reset--",
                  CreatedAt = DateTime.UtcNow  
                };
                await context.AddAsync(invoice);
                await context.SaveChangesAsync();
            }
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}