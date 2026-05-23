namespace Step022.Application.Orders;

internal class OrderService : IOrderService
{

    private readonly IOrderRepository _orderRepo;
    private readonly IProductRepository _productRepo;

    public OrderService(IOrderRepository orderRepo, IProductRepository productRepo)
    {
        _orderRepo = orderRepo;
        _productRepo = productRepo;
    }


    // ============================================================
    // Create Order
    // ============================================================
    public Order CreateOrder(int userId, List<(int ProductId, int Quantity)> items)
    {
        if (items == null || items.Count == 0)
            throw new ArgumentException("Order must contain at least one item.");

        var orderItems = new List<OrderItem>();

        foreach (var (productId, quantity) in items)
        {
            // چک موجودی
            //TODO : Refactor -> _productRepo.GetByIdOrThrow
            var product = _productRepo.GetById(productId)
                ?? throw new KeyNotFoundException($"Product {productId} not found.");

            // ⭐ Product خودش اعتبارسنجی می‌کنه و کم می‌کنه
            product.ReduceStock(quantity);
            _productRepo.Update(product);

            // ساخت OrderItem
            var orderItem = OrderItem.Create(productId, product.Name, quantity, product.Price);
            orderItems.Add(orderItem);
        }

        // ⭐ Order خودش رو می‌سازه
        var order = Order.Create(userId, orderItems);
        _orderRepo.Add(order);

        return order;
    }


    // ============================================================
    // Get Orders
    // ============================================================
    public Order GetOrder(int id)
    {
        var order = _orderRepo.GetById(id)
            ?? throw new KeyNotFoundException($"Order {id} not found.");

        return order;
    }

    public List<Order> GetAllOrders()
    {
        return _orderRepo.GetAll();
    }

    public List<Order> GetMyOrders(int userId)
    {
        return _orderRepo.GetByUserId(userId);
    }


    // ============================================================
    // Process Order - State Machine رو از Entity می‌خونه
    // ============================================================
    public Order ConfirmOrder(int orderId)
    {
        var order = GetOrder(orderId);

        // ⭐ Entity خودش State Machine رو اجرا می‌کنه
        order.Confirm();
        _orderRepo.Update(order);

        return order;
    }

    public Order ShipOrder(int orderId)
    {
        var order = GetOrder(orderId);
        order.Ship();
        _orderRepo.Update(order);

        return order;
    }

    public Order DeliverOrder(int orderId)
    {
        var order = GetOrder(orderId);
        order.Deliver();
        _orderRepo.Update(order);

        return order;
    }

    // ============================================================
    // Cancel Order - با برگشت موجودی
    // ============================================================
    public Order CancelOrder(int orderId)
    {
        var order = GetOrder(orderId);

        // ⭐ Order خودش اعتبارسنجی می‌کنه و کنسل می‌شه
        order.Cancel();

        // ⭐ برگشت موجودی - Application این کار رو هماهنگ می‌کنه
        var itemsToReturn = order.GetItemsToReturn();
        foreach (var (productId, quantity) in itemsToReturn)
        {
            var product = _productRepo.GetById(productId);
            if (product != null)
            {
                product.IncreaseStock(quantity);
                _productRepo.Update(product);
            }
        }

        _orderRepo.Update(order);

        return order;
    }


    // ============================================================
    // Cancel My Order - با چک مالکیت
    // ============================================================
    public Order CancelMyOrder(int orderId, int userId)
    {
        var order = GetOrder(orderId);

        if (!order.BelongsTo(userId))
            throw new UnauthorizedAccessException("You can only cancel your own orders.");

        return CancelOrder(orderId);
    }

}
