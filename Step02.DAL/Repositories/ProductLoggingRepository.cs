using Step02.BLL.Entities;
using Step02.BLL.Repositorties;
using Step02.DAL.Data;

namespace Step02.DAL.Repositories;

public class ProductLoggingRepository : IProductRepository
{
    private readonly IProductRepository _repository;
    public ProductLoggingRepository(DatabaseContext dbContext)
    {
        _repository = new ProductRepository(dbContext);
    }

    public void Add(Product product)
    {
        Console.WriteLine($"[LOG-REPO] : try to add a new product. ");
        _repository.Add(product);   

    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public List<Product> GetAll()
    {
        Console.WriteLine($"[LOG-REPO] : try to Get all products. ");
       return _repository.GetAll();
    }

    public Product GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(Product product)
    {
        throw new NotImplementedException();
    }
}
