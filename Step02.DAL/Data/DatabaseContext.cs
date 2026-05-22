using Step02.DAL.Entities;

namespace Step02.DAL.Data;

public class DatabaseContext
{
    public List<Product> Products = new List<Product>();
    public List<Order> Orders = new List<Order>();
    public List<User> Users = new List<User>();


    public static int ProductNextId = 1;
    public static int UserNextId = 1;
    public static int OrderNextId = 1;
}
