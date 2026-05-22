using Step02.DAL.Entities;

namespace Step02.DAL.Repositorties;

public interface IOrderRepository
{
    void Add(Order order);
    void Update(Order order);
    Order GetById(int id);
    List<Order> GetAll();
    List<Order> GetByStatus(OrderStatus status);

    List<Order> GetByUserId(int userId);
}
