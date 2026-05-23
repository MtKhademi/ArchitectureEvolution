namespace Step022.Application.Products;


public interface IProductService
{
    Product CreateProduct(string name, decimal price, int stockQuantity);
    Product GetProduct(int id);
    List<Product> GetAllProducts();
    Product UpdateProduct(int id, string name, decimal price);
    void DeleteProduct(int id);
    void ReduceStock(int productId, int quantity);
}
