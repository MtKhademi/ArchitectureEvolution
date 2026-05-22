using Step02.BLL.Entities;
using Step02.BLL.Repositorties;

namespace Step02.BLL;

public class ProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }


    public Product AddNewProduct(string name, decimal price, int quantity)
    {
        if (_productRepository.GetAll().Any(x => x.Name.Equals(name))) throw new Exception("Name must be unique.");

        var product = new Product
        {
            Name = name,
            Price = price,
            StockQuantity = quantity
        };
        _productRepository.Add(product);
        return product;
    }
    public Product UpdateProduct(int productId, string name, decimal price, int quantity)
    {
        var product = new Product
        {
            Id = productId,
            Name = name,
            Price = price,
            StockQuantity = quantity
        };
        _productRepository.Update(product);
        return product;
    }

    public Product DeleteProduct(int productId)
    {
        var product = _productRepository.GetById(productId);
        _productRepository.Delete(productId);

        return product;
    }

    public List<Product> GetAllProduct() => _productRepository.GetAll();

    public Product GetProductById(int productId) => _productRepository.GetById(productId);
}
