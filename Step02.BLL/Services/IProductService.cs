using Step02.DAL.Entities;

public interface IProductService
{
    Product AddNewProduct(string name, decimal price, int quantity);
    Product UpdateProduct(int productId, string name, decimal price, int quantity);
    Product DeleteProduct(int productId);
    List<Product> GetAllProduct();
    Product GetProductById(int productId);
}
