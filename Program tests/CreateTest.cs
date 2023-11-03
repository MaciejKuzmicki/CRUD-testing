using System;
using CRUD_test;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CRUD_test.Tests
{
    public class DbCreateTests
    {
        [Fact]
        public void Create_AddsProductToDatabase()
        {
            using (var productDbContext = CreateDbContext())
            {
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
        public void Create_ThrowsArgumentNullExceptionForNullProduct()
        {
            using (var productDbContext = CreateDbContext())
            {
                Product nullProduct = null;

                Assert.Throws<ArgumentNullException>(() => Program.Create(nullProduct, productDbContext));
            }
        }

        [Fact]
        public void Create_ThrowsArgumentExceptionForEmptyName()
        {
            using (var productDbContext = CreateDbContext())
            {
                var product = new Product
                {
                    Name = string.Empty,
                    Price = 19.99,
                    Weight = 5.0,
                    Description = "A new product for testing",
                };

                Assert.Throws<ArgumentException>(() => Program.Create(product, productDbContext));
            }
        }

        [Fact]
        public void Create_ThrowsArgumentExceptionForNonPositivePrice()
        {
            using (var productDbContext = CreateDbContext())
            {
                var product = new Product
                {
                    Name = "NewProduct",
                    Price = 0,
                    Weight = 5.0,
                    Description = "A new product for testing",
                };

                Assert.Throws<ArgumentException>(() => Program.Create(product, productDbContext));
            }
        }

        [Fact]
        public void Create_ThrowsArgumentExceptionForNonPositiveWeight()
        {
            using (var productDbContext = CreateDbContext())
            {
                var product = new Product
                {
                    Name = "NewProduct",
                    Price = 19.99,
                    Weight = 0,
                    Description = "A new product for testing",
                };

                Assert.Throws<ArgumentException>(() => Program.Create(product, productDbContext));
            }
        }

        private ProductDbContext CreateDbContext()
        {
            var context = new ProductDbContext();
            context.Database.EnsureCreated();
            return context;
        }
    }
}
