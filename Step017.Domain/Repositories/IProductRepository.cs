namespace Step017.Domain.Repositories;

public interface IProductRepository
{
    Product GetById(int id);
    List<Product> GetAll();
    void Add(Product product);
    void Update(Product product);
    void Delete(int id);
    bool Exists(int id);
}
