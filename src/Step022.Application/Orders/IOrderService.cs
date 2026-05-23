namespace Step022.Application.Orders;

public interface IOrderService
{
    Order CreateOrder(int userId, List<(int ProductId, int Quantity)> items);
    Order GetOrder(int id);
    List<Order> GetAllOrders();
    List<Order> GetMyOrders(int userId);
    Order ConfirmOrder(int orderId);
    Order ShipOrder(int orderId);
    Order DeliverOrder(int orderId);
    Order CancelOrder(int orderId);
    Order CancelMyOrder(int orderId, int userId);
}
