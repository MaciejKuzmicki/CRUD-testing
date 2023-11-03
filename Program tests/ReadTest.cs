using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CRUD_test.Tests
{
    public class DbReadTests : IDisposable
    {
        private ProductDbContext _db;

        public DbReadTests()
        {
            _db = new ProductDbContext();
            _db.Database.EnsureCreated();
        }

        public void Dispose() => _db.Dispose();

        [Fact]
        public void Read_ReturnsArgumentExceptionWhenZeroID()
        {
            Assert.Throws<ArgumentException>(() => Program.Get(0, _db));
        }

        [Fact]
        public void Read_ReturnsArgumentExceptionWhenNegativeID()
        {
            Assert.Throws<ArgumentException>(() => Program.Get(-1, _db));
        }

        [Fact]
        public void Read_ReadProduct()
        {
            string name = "Test";
            double price = 1.23;
            double weight = 3.21;
            string description = "produkt testowy";

            Product newPr = new Product
            {
                Name = name,
                Price = price,
                Weight = weight,
                Description = description,
            };

            Program.Create(newPr, _db);

            Product getPr = Program.Get(newPr.Id, _db);

            Assert.Equal(getPr, newPr);
            Assert.Equal(getPr.Name, name);
            Assert.Equal(getPr.Price, price);
            Assert.Equal(getPr.Weight, weight);
            Assert.Equal(getPr.Description, description);
        }

        [Fact]
        public void Read_ReadMultipleProducts()
        {
            string name1 = "Test1";
            double price1 = 1.23;
            double weight1 = 3.21;
            string description1 = "produkt testowy 1";
            Product newPr1 = new Product
            {
                Name = name1,
                Price = price1,
                Weight = weight1,
                Description = description1,
            };
            Program.Create(newPr1, _db);

            string name2 = "Test2";
            double price2 = 9.87;
            double weight2 = 7.89;
            string description2 = "produkt testowy 2";
            Product newPr2 = new Product
            {
                Name = name2,
                Price = price2,
                Weight = weight2,
                Description = description2,
            };
            Program.Create(newPr2, _db);

            Product getPr1 = Program.Get(newPr1.Id, _db);
            Product getPr2 = Program.Get(newPr2.Id, _db);

            Assert.Equal(getPr1, newPr1);
            Assert.Equal(getPr1.Name, name1);
            Assert.Equal(getPr1.Price, price1);
            Assert.Equal(getPr1.Weight, weight1);
            Assert.Equal(getPr1.Description, description1);

            Assert.Equal(getPr2, newPr2);
            Assert.Equal(getPr2.Name, name2);
            Assert.Equal(getPr2.Price, price2);
            Assert.Equal(getPr2.Weight, weight2);
            Assert.Equal(getPr2.Description, description2);
        }

        [Fact]
        public void Read_ReturnsNullWhenIdIsPositiveButNotExisting()
        {
            // Dodanie produktu do bazy
            Product newPr = new Product
            {
                Name = "Test",
                Price = 1.23,
                Weight = 3.21,
                Description = "produkt testowy",
            };
            Program.Create(newPr, _db);

            // Pobranie największego id z tabeli
            int maxId = _db.Products.Max(p => p.Id);

            Assert.Null(Program.Get(maxId + 1, _db));
        }

        [Fact]
        public void Read_ThrowsNullReferenceExceptionWhenDBContextIsNull()
        {
            Assert.Throws<NullReferenceException>(() => Program.Get(1, null));
        }
    }
}
