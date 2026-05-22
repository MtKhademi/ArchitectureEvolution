namespace Step02.DAL.Entities;

public class Order
{
    public int Id { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public int UserId { get; set; }

    public decimal TotalPrice => Items.Sum(item => item.TotalPrice);

    public override string ToString()
        => $"Id : {Id} | {OrderDate:yy-mm-dd hh:mm:ss} | items : {Items.Count} | {TotalPrice:0000.00}";

}

public class OrderItem
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public decimal TotalPrice => Quantity * UnitPrice;
}

public enum OrderStatus
{
    Pending,
    Confirmed,
    Shipped,
    Delivered,
    Cancelled
}
