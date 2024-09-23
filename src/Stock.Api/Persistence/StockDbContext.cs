using Microsoft.EntityFrameworkCore;
using Stock.Api.Persistence.Configurations;

namespace Stock.Api.Persistence;

public class StockDbContext : DbContext
{
    private readonly string _connectionString;

    public StockDbContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("sql");

        Database.EnsureCreated();
    }

    public DbSet<Domain.Models.Stock> Stocks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new StockConfiguration().Configure(modelBuilder.Entity<Domain.Models.Stock>());

        base.OnModelCreating(modelBuilder);
    }
}
