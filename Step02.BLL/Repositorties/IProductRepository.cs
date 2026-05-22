using Step02.BLL.Entities;

namespace Step02.BLL.Repositorties;

public interface IProductRepository
{
    Product GetById(int id);
    void Add(Product product);
    void Update(Product product);
    void Delete(int id);
    List<Product> GetAll();
}
