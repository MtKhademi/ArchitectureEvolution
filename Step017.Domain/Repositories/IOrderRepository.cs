namespace Step017.Domain.Repositories;

public interface IOrderRepository
{
    Order GetById(int id);
    List<Order> GetAll();
    List<Order> GetByUserId(int userId);
    List<Order> GetByStatus(OrderStatus status);
    void Add(Order order);
    void Update(Order order);
}
