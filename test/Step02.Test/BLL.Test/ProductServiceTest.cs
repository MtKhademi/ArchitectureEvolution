//using FluentAssertions;
//using Step02.BLL;
//using Step02.DAL.Data;
//using Step02.DAL.Repositories;

//namespace Step02.Test.BLL.Test;

//public class ProductServiceTest
//{
//    [Fact]
//    public void Should_to_be_able_add_new_product()
//    {
//        var dbContext = new DatabaseContext();
//        var productRepository = new ProductRepository(dbContext);
//        var productService = new ProductService(productRepository);

//        var productResult = productService.AddNewProduct("Pro001", 100, 10);

//        var productExpect = productService.GetProductById(productResult.Id);

//        productExpect.Should().NotBeNull();
//        productExpect.Name.Should().Be(productResult.Name);
//    }


//    [Fact]
//    public void Should_to_be_able_add_new_product_when_empty_name()
//    {
//        var dbContext = new DatabaseContext();
//        var productRepository = new ProductRepository(dbContext);
//        var productService = new ProductService(productRepository);

//        var actException = () => productService.AddNewProduct("", 100, 10);


//        actException.Should().Throw<ArgumentNullException>();
//        var products = productService.GetAllProduct();
//        products.Should().HaveCount(0);
//    }

//    [Fact]
//    public void Should_not_to_be_able_add_new_product_with_duplicate_name()
//    {
//        var dbContext = new DatabaseContext();
//        var productRepository = new ProductRepository(dbContext);
//        var productService = new ProductService(productRepository);

//        var productResult = productService.AddNewProduct("Pro001", 100, 10);

//        var productExpect = productService.GetProductById(1);

//        productExpect.Should().NotBeNull();
//        productExpect.Name.Should().Be(productResult.Name);


//        var actException = () => productService.AddNewProduct("Pro001", 100, 10);
//        actException.Should().Throw<Exception>()
//            .WithMessage("Name must be unique.");
//    }
//}
