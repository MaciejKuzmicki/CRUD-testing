using System;
using CRUD_test;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Moq;

namespace CRUD_test.Tests
{
    public class DbDeleteTests
    {

        [Fact]
        public void CanDeleteOneExistingProduct()
        {
            // given
            var productDbContext = new ProductDbContext();
            var newProduct = new Product
            {
                Name = "test",
                Price = 1.0,
                Weight = 1.0,
                Description = "test",
            };
            Program.Create(newProduct, productDbContext);

            // when
            Program.Delete(newProduct.Id, productDbContext);

            // then
            var deletedProduct = Program.Get(newProduct.Id, productDbContext);
            Assert.Null(deletedProduct);
        }

        [Fact]
        public void CanDeleteMultipleProducts()
        {
            //given
            var productDbContext = new ProductDbContext();

            //when
            var newProduct1 = new Product
            {
                Name = "test",
                Price = 1.0,
                Weight = 1.0,
                Description = "test",
            };
            Program.Create(newProduct1, productDbContext);
            var newProduct2 = new Product
            {
                Name = "test",
                Price = 1.0,
                Weight = 1.0,
                Description = "test",
            };
            Program.Create(newProduct2, productDbContext);

            var cratedIds = new[] { newProduct1.Id, newProduct2.Id };
            Program.Delete(newProduct1.Id, productDbContext);
            Program.Delete(newProduct2.Id, productDbContext);

            //then
            foreach (var productId in cratedIds)
            {
                var deletedProduct = Program.Get(productId, productDbContext);
                Assert.Null(deletedProduct);
            }
        }
        
        [Fact]
        public void CannotDeleteNonExistingProduct()
        {
            //given
            var productDbContext = new ProductDbContext();
            var nonExistingProductId = 100000; //random non existent id 

            //when
            
            //then
            Assert.Throws<InvalidOperationException>(() => Program.Delete(nonExistingProductId, productDbContext));
        }
        
        [Fact]
        public void CannotDeleteProductWithNegativeId()
        {
            //given
            var productDbContext = new ProductDbContext();
            var negativeId = -10000; 

            //when
            
            //then
            Assert.Throws<ArgumentException>(() => Program.Delete(negativeId, productDbContext));
        }
        
        [Fact]
        public void CannotDeleteProductTwice()
        {
            //given
            var productDbContext = new ProductDbContext();
            var product = new Product
            {
                Name = "test",
                Price = 1.0,
                Weight = 1.0,
                Description = "test",
            };

            Program.Create(product, productDbContext);

            //when
            Program.Delete(product.Id, productDbContext);

            //then
            var ex = Assert.Throws<InvalidOperationException>(() => Program.Delete(product.Id, productDbContext));
            Assert.Contains($"Product with ID {product.Id} not found.", ex.Message);
        }
    }
}
