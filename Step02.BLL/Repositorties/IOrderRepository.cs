using Step02.BLL.Entities;

namespace Step02.BLL.Repositorties;

public interface IOrderRepository
{
    void Add(Order order);
    void Update(Order order);
    Order GetById(int id);
    List<Order> GetAll();
    List<Order> GetByStatus(OrderStatus status);

    List<Order> GetByUserId(int userId);
}
