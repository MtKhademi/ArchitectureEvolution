using Step02.DAL.Entities;

namespace Step02.DAL.Repositorties;

public interface IProductRepository
{
    Product GetById(int id);
    void Add(Product product);
    void Update(Product product);
    void Delete(int id);
    List<Product> GetAll();
}
