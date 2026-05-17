using FluentAssertions;
using Step02.BLL;

namespace Step02.Test.BLL.Test;

public class ProductServiceTest
{
    [Fact]
    public void Should_to_be_able_add_new_product()
    {
        var productService = new ProductService(new DAL.Data.DatabaseContext());

        var productResult = productService.AddNewProduct("Pro001", 100, 10);

        var productExpect = productService.GetProductById(1);

        productExpect.Should().NotBeNull();
        productExpect.Name.Should().Be(productResult.Name);
    }


    [Fact]
    public void Should_to_be_able_add_new_product_when_empty_name()
    {
        var productService = new ProductService(new DAL.Data.DatabaseContext());

        var actException = () => productService.AddNewProduct("", 100, 10);


        actException.Should().Throw<ArgumentNullException>();
        var products = productService.GetAllProduct();
        products.Should().HaveCount(0);
    }
}
