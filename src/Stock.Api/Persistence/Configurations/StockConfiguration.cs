using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stock.Api.Persistence.Configurations;

public class StockConfiguration : IEntityTypeConfiguration<Domain.Models.Stock>
{
    public void Configure(EntityTypeBuilder<Domain.Models.Stock> builder)
    {
        builder.HasKey(s => s.Barcode);

        builder.HasData(new Domain.Models.Stock
        {
            Barcode = "11111",
            Quantity = 100
        }, new Domain.Models.Stock
        {
            Barcode = "22222",
            Quantity = 100
        }, new Domain.Models.Stock
        {
            Barcode = "33333",
            Quantity = 100
        });
    }
}
