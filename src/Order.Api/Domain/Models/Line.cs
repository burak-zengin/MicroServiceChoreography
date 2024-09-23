namespace Order.Api.Domain.Models;

public class Line
{
    public int Id { get; set; }

    public string Barcode { get; set; }

    public int Quantity { get; set; }
}
