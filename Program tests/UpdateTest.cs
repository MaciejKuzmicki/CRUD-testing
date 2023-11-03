using System;
using CRUD_test;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CRUD_test.Tests
{
    public class DbUpdateTests
    {
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

        [Fact]
        public void Update_ThrowsArgumentNullExceptionForNullProduct()
        {
            using (var productDbContext = CreateDbContext())
            {
                Product nullProduct = null;

                Assert.Throws<ArgumentNullException>(() => Program.Update(nullProduct, productDbContext));
            }
        }

        [Fact]
        public void Update_ThrowsArgumentExceptionForNonPositiveId()
        {
            using (var productDbContext = CreateDbContext())
            {
                var updatedProduct = new Product
                {
                    Id = 0,
                    Name = "UpdatedProduct",
                    Price = 24.99,
                    Weight = 6.0,
                    Description = "An updated product for testing",
                };

                Assert.Throws<ArgumentException>(() => Program.Update(updatedProduct, productDbContext));
            }
        }

        [Fact]
        public void Update_ThrowsInvalidOperationExceptionForNonExistingProduct()
        {
            using (var productDbContext = CreateDbContext())
            {
                var updatedProduct = new Product
                {
                    Id = 999,
                    Name = "UpdatedProduct",
                    Price = 24.99,
                    Weight = 6.0,
                    Description = "An updated product for testing",
                };

                Assert.Throws<InvalidOperationException>(() => Program.Update(updatedProduct, productDbContext));
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
