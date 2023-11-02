using System;
using CRUD_test;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CRUD_test.Tests
{
    public class ProgramTests
    {
        [Fact]
        public void Create_AddsProductToDatabase()
        {
            using (var productDbContext = CreateDbContext())
            {
                var program = new Program();

                var newProduct = new Product
                {
                    Name = "NewProduct",
                    Price = 19.99,
                    Weight = 5.0,
                    Description = "A new product for testing",
                };

                Program.Create(newProduct, productDbContext);

                var retrievedProduct = Program.Get(newProduct.Id, productDbContext);
                Assert.NotNull(retrievedProduct);
                Assert.Equal(newProduct.Name, retrievedProduct.Name);
                Assert.Equal(newProduct.Price, retrievedProduct.Price);
                Assert.Equal(newProduct.Weight, retrievedProduct.Weight);
                Assert.Equal(newProduct.Description, retrievedProduct.Description);
            }
        }

        [Fact]
        public void Update_UpdatesProductInDatabase()
        {
            using (var productDbContext = CreateDbContext())
            {
                var existingProduct = CreateAndRetrieveProduct(productDbContext, "InitialProduct", 19.99, 5.0, "An initial product for testing");

                var updatedProduct = new Product
                {
                    Id = existingProduct.Id,
                    Name = "UpdatedProduct",
                    Price = 24.99,
                    Weight = 6.0,
                    Description = "An updated product for testing",
                };

                Program.Update(updatedProduct, productDbContext);

                var retrievedProduct = Program.Get(existingProduct.Id, productDbContext);

                Assert.NotNull(retrievedProduct);
                Assert.Equal(updatedProduct.Name, retrievedProduct.Name);
                Assert.Equal(updatedProduct.Price, retrievedProduct.Price);
                Assert.Equal(updatedProduct.Weight, retrievedProduct.Weight);
                Assert.Equal(updatedProduct.Description, retrievedProduct.Description);
            }
        }




        private ProductDbContext CreateDbContext()
        {
            var context = new ProductDbContext();
            context.Database.EnsureCreated();
            return context;
        }

        private Product CreateAndRetrieveProduct(ProductDbContext productDbContext, string name, double price, double weight, string description)
        {
            var program = new Program();

            var newProduct = new Product
            {
                Name = name,
                Price = price,
                Weight = weight,
                Description = description,
            };

            Program.Create(newProduct, productDbContext);

            return Program.Get(newProduct.Id, productDbContext);
        }
    }
}
