using Step02.BLL.Entities;
using Step02.BLL.Repositorties;
using System.Threading.Channels;

namespace Step02.BLL.Services;


//قوانین کسب‌و‌کار:

//❌ وقتی Delivered یا Shipped شد، دیگه نمی‌شه Cancel کرد
//❌ سفارش Cancelled رو نمی‌شه دوباره فعال کرد
//❌ نمی‌شه از Pending مستقیم به Delivered رفت(باید ترتیب طی بشه)
//✅ فقط Pending و Confirmed قابل کنسل شدن هستنAIN
//✅ موقع Confirm باید دوباره موجودی چک بشه(ممکنه موجودی کم شده باشه)

public interface IOrderService
{
    Order CreateOrder(int userId, List<int> productIds, List<int> quantities);
    Order GetOrderById(int orderId);
    List<Order> GetAllOrders();
    List<Order> GetOrdersByStatus(OrderStatus status);
    Order UpdateOrderStatus(int orderId, OrderStatus newStatus);
    Order CancelOrder(int orderId);

    List<Order> GetMyOrders(int userId);
    Order CancelMyOrder(int orderId, int userId);
}

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;

    public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IUserRepository userRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _userRepository = userRepository;
    }



    public Order CancelOrder(int orderId)
    {
        return UpdateOrderStatus(orderId, OrderStatus.Cancelled);
    }

    public Order CreateOrder(int userId, List<int> productIds, List<int> quantities)
    {


        //-> Validation
        if (productIds == null || productIds.Count == 0) throw new ArgumentException("Order must contain at least one product.");
        if (quantities == null || quantities.Count != productIds.Count) throw new ArgumentException("Product count and quantity count must match.");


        //- Validation user
        var user = _userRepository.GetById(userId);


        var orderItems = new List<OrderItem>();
        for (int i = 0; i < productIds.Count; i++)
        {
            var productId = productIds[i];
            var quantity = quantities[i];

            if (quantity <= 0) throw new ArgumentException($"Invalid quantity ({quantity}) for product ID {productId}.");

            var product = _productRepository.GetById(productId);

            //-> Validation Base Count quantity
            if (product.StockQuantity < quantity)
                throw new InvalidOperationException(
                    $"Not enough stock for '{productId}:{product.Name}'. " +
                    $"Available: {product.StockQuantity}, Requested: {quantity}");


            orderItems.Add(new OrderItem
            {
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = product.Price
            });

            product.StockQuantity -= quantity;
            _productRepository.Update(product);
        }

        var orderEntity = new Order()
        {
            UserId = user.Id,
            Items = orderItems
        };
        _orderRepository.Add(orderEntity);
        return orderEntity;

    }

    public List<Order> GetAllOrders() => _orderRepository.GetAll();

    public Order GetOrderById(int orderId)
    {
        var order = _orderRepository.GetById(orderId);
        if (order == null)
            throw new KeyNotFoundException($"Order with ID {orderId} not found.");

        return order;
    }

    public List<Order> GetOrdersByStatus(OrderStatus status) => _orderRepository.GetByStatus(status);

    public Order UpdateOrderStatus(int orderId, OrderStatus newStatus)
    {
        var orderEntity = GetOrderById(orderId);

        //-> Valiodation
        if (newStatus == OrderStatus.Cancelled && (orderEntity.Status == OrderStatus.Shipped || orderEntity.Status == OrderStatus.Delivered))
        {
            throw new InvalidOperationException($"Order is already : {orderEntity.Status}, can not cancelled.");
        }

        if (newStatus == OrderStatus.Cancelled && (orderEntity.Status == OrderStatus.Pending || orderEntity.Status == OrderStatus.Confirmed))
        {
            throw new InvalidOperationException($"Order is already : {orderEntity.Status}, can not cancelled.");
        }

        ValidationStatusTransaction(orderEntity.Status, newStatus);

        orderEntity.Status = newStatus;
        _orderRepository.Update(orderEntity);

        return orderEntity;
    }

    private void ValidationStatusTransaction(OrderStatus currentStatus, OrderStatus newStatus)
    {
        var allowedTransactions = new Dictionary<OrderStatus, List<OrderStatus>>
        {
            { OrderStatus.Pending, new List<OrderStatus> { OrderStatus.Confirmed, OrderStatus.Cancelled } },
            { OrderStatus.Confirmed, new List<OrderStatus> { OrderStatus.Shipped, OrderStatus.Cancelled } },
            { OrderStatus.Shipped, new List<OrderStatus> { OrderStatus.Delivered } }
        };


        if (!allowedTransactions.ContainsKey(currentStatus))
            throw new InvalidOperationException($"Unknown order status: {currentStatus}");

        if (!allowedTransactions[currentStatus].Contains(newStatus))
            throw new InvalidOperationException(
                $"Cannot change order status from '{currentStatus}' to '{newStatus}'. " +
                $"Allowed: {string.Join(", ", allowedTransactions[currentStatus])}");

    }



    // ============================================================
    // GetMyOrders - فقط سفارش‌های کاربر جاری
    // ============================================================
    public List<Order> GetMyOrders(int userId)
    {
        return _orderRepository.GetByUserId(userId);
    }

    // ============================================================
    // CancelMyOrder - فقط سفارش خودش رو می‌تونه کنسل کنه
    // ============================================================
    public Order CancelMyOrder(int orderId, int userId)
    {
        var order = GetOrderById(orderId);

        // ⭐ چک مالکیت
        if (order.UserId != userId)
            throw new UnauthorizedAccessException("You can only cancel your own orders.");

        return CancelOrder(orderId); // از متد قبلی استفاده می‌کنیم
    }
}