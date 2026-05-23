//===============================
// Create by mt.khademi
//==============================

namespace Step017.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; private set; } = DateTime.Now;
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public int UserId { get; private set; }
    public decimal TotalPrice => Items.Sum(item => item.TotalPrice);

    public override string ToString()
        => $"Id : {Id} | {OrderDate:yy-mm-dd hh:mm:ss} | items : {Items.Count} | {TotalPrice:0000.00}";


    private List<OrderItem> _items = new();
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();


    private Order()
    {

    }


    public static Order Create(int userId, List<OrderItem> items)
    {
        if (userId <= 0)
            throw new ArgumentException("Invalid user ID.");

        if (items == null || items.Count == 0)
            throw new ArgumentException("Order must contain at least one item.");

        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.Now,
            Status = OrderStatus.Pending,
            _items = items
        };

        return order;
    }


    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException(
                $"Cannot confirm order in '{Status}' status. Only Pending orders can be confirmed.");

        Status = OrderStatus.Confirmed;
    }

    public void Ship()
    {
        if (Status != OrderStatus.Confirmed)
            throw new InvalidOperationException(
                $"Cannot ship order in '{Status}' status. Only Confirmed orders can be shipped.");

        Status = OrderStatus.Shipped;
    }

    public void Deliver()
    {
        if (Status != OrderStatus.Shipped)
            throw new InvalidOperationException(
                $"Cannot deliver order in '{Status}' status. Only Shipped orders can be delivered.");

        Status = OrderStatus.Delivered;
    }


    public void Cancel()
    {
        if (!CanBeCancelled())
            throw new InvalidOperationException(
                $"Cannot cancel order in '{Status}' status. Only Pending or Confirmed orders can be cancelled.");

        Status = OrderStatus.Cancelled;
    }

    public List<(int ProductId, int Quantity)> GetItemsToReturn()
    {
        if (Status != OrderStatus.Cancelled)
            return new List<(int, int)>();

        return _items.Select(i => i.GetReturnInfo()).ToList();
    }

    public bool BelongsTo(int userId)
    {
        return UserId == userId;
    }

    public bool CanBeCancelled()
    {
        return Status == OrderStatus.Pending || Status == OrderStatus.Confirmed;
    }

}

public class OrderItem
{
    // چرا ؟؟؟؟
    public int ProductId { get; private set; }
    public string ProductName { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    public decimal TotalPrice => Quantity * UnitPrice;

    private OrderItem()
    {

    }

    public static OrderItem Create(int productId, string productName, decimal unitPrice, int quantity)
    {
        if (productId <= 0)
            throw new ArgumentException("Invalid product ID.");


        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Product name is required.");

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive.");

        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative.");

        return new OrderItem
        {
            ProductId = productId,
            ProductName = productName,
            Quantity = quantity,
            UnitPrice = unitPrice
        };
    }


    public (int ProductId, int Quantity) GetReturnInfo()
    {
        return (ProductId, Quantity);
    }

}
