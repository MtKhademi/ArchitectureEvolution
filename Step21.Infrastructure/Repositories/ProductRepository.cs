namespace Step03.Infrastructure.Repositories;

public class ProductRepository : IProductRepository  // ⭐ از Domain میاد!
{
    private readonly DatabaseContext _db; 

    public ProductRepository(DatabaseContext db)
    {
        _db = db;
    }

    public Product GetById(int id)
    {
        return _db.Products.FirstOrDefault(p => p.Id == id);
    }

    public List<Product> GetAll()
    {
        return _db.Products.ToList();
    }

    public void Add(Product product)
    {
        // ⭐ Id رو اینجا می‌دیم - Domain نمی‌دونه Id چطور ساخته می‌شه
        product.Id = DatabaseContext.ProductNextId++;
        _db.Products.Add(product);
    }

    public void Update(Product product)
    {
        var index = _db.Products.FindIndex(p => p.Id == product.Id);
        if (index != -1)
            _db.Products[index] = product;
    }

    public void Delete(int id)
    {
        var product = GetById(id);
        if (product != null)
            _db.Products.Remove(product);
    }

    public bool Exists(int id)
    {
        return _db.Products.Any(p => p.Id == id);
    }
}