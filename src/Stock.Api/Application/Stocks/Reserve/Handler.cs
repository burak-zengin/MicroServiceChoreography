using Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Results;
using Stock.Api.Persistence;

namespace Stock.Api.Application.Stocks.Reserve;

public class Handler(StockDbContext context)
{
    public async Task<Result> Handle(Command command)
    {
        foreach (var loopLine in command.Lines)
        {
            if (context.Stocks.Any(s => s.Barcode == loopLine.Barcode) == false)
            {
                return new Result
                {
                    Failed = true,
                    Message = $"Stock not found. Barcode: {loopLine.Barcode}."
                };
            }

            if (context.Stocks.Any(s => s.Barcode == loopLine.Barcode && s.Quantity < loopLine.Quantity))
            {
                return new Result
                {
                    Failed = true,
                    Message = $"Insufficient stock. Barcode: {loopLine.Barcode}."
                };
            }
        }

        foreach (var loopLine in command.Lines)
        {
            context
                .Stocks
                .Where(s => s.Barcode == loopLine.Barcode)
                .ExecuteUpdate(spc => spc.SetProperty(s => s.Quantity, s => s.Quantity - loopLine.Quantity));
        }

        return new Result();
    }
}