using Microsoft.EntityFrameworkCore;

namespace Order.Api.Persistence;

public class OrderDbContext : DbContext
{
    private readonly string _connectionString;

    public OrderDbContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("sql");

        Database.EnsureCreated();
    }

    public DbSet<Domain.Models.Order> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);

        base.OnConfiguring(optionsBuilder);
    }
}
