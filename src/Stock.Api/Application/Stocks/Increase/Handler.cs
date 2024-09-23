using Microsoft.EntityFrameworkCore;
using Results;
using Stock.Api.Persistence;

namespace Stock.Api.Application.Stocks.Increase;

public class Handler(StockDbContext context)
{
    public Result Handle(Command command)
    {
        foreach (var loopLine in command.Lines)
        {
            context
                .Stocks
                .Where(s => s.Barcode == loopLine.Barcode)
                .ExecuteUpdate(spc => spc.SetProperty(s => s.Quantity, s => s.Quantity + loopLine.Quantity));
        }

        return new Result();
    }
}