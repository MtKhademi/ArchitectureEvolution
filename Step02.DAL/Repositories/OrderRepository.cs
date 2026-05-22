using Step02.BLL.Entities;
using Step02.BLL.Repositorties;
using Step02.DAL.Data;

namespace Step02.DAL.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly DatabaseContext _db;

    public OrderRepository(DatabaseContext db)
    {
        _db = db;
    }

    public void Add(Order order)
    {
        order.Id = DatabaseContext.OrderNextId++;
        _db.Orders.Add(order);
    }

    public void Update(Order order)
    {
        var index = _db.Orders.FindIndex(o => o.Id == order.Id);
        if (index != -1)
            _db.Orders[index] = order;
    }

    public Order GetById(int id)
    {
        return _db.Orders.FirstOrDefault(o => o.Id == id);
    }

    public List<Order> GetAll()
    {
        return _db.Orders
            .OrderByDescending(o => o.OrderDate)
            .ToList();
    }

    public List<Order> GetByStatus(OrderStatus status)
    {
        return _db.Orders
            .Where(o => o.Status == status)
            .OrderByDescending(o => o.OrderDate)
            .ToList();
    }

    public List<Order> GetByUserId(int userId)
    {
        return _db.Orders
             .Where(o => o.UserId == userId)
             .OrderByDescending(o => o.OrderDate)
             .ToList();
    }
}
