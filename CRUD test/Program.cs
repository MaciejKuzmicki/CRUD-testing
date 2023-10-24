using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.InteropServices;

namespace CRUD_test
{
    class Program
    {
        static void Main(string[] args)
        {
            ProductDbContext productDbContext = new ProductDbContext();
            Product product = new Product();
            product.Name = "Test";
            product.Price = 10.4;
            product.Weight = 10;
            product.Description = "Test";
            //Create(product, productDbContext);
            Product product1 = new Product();
            product1.Id = 3;
            product1.Name = "test2";
            product1.Price = 10.5;
            product1.Weight = 4.5;
            product1.Description = "Test2";
            //Update(product1, productDbContext);
            //Show(Get(3, productDbContext));
            Delete(3, productDbContext);
            productDbContext.SaveChanges();
            List(productDbContext);
        }

        public static void List(ProductDbContext productDbContext)
        {
            foreach(var item in  productDbContext.Products)
            {
                Console.WriteLine("Id: " + item.Id + "Name: " + item.Name + "Description: " + item.Description + "Price: " + item.Price + "Weight: " + item.Weight);
            }
        }

        public static void Show(Product product)
        {
            Console.WriteLine("Id: " + product.Id + "Name: " + product.Name + "Description: " + product.Description + "Price: " + product.Price + "Weight: " + product.Weight);
        }

        public static void Create(Product product, ProductDbContext productDbContext)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (string.IsNullOrEmpty(product.Name))
                throw new ArgumentException("Product Name is required.", nameof(product.Name));

            if (product.Price <= 0)
                throw new ArgumentException("Product Price must be greater than zero.", nameof(product.Price));

            if (product.Weight <= 0)
                throw new ArgumentException("Product Weight must be greater than zero.", nameof(product.Weight));

            productDbContext.Products.Add(product);
            productDbContext.SaveChanges();
        }

        public static Product Get(int productId, ProductDbContext productDbContext)
        {
            if (productId <= 0)
                throw new ArgumentException("Invalid product ID", nameof(productId));

            return productDbContext.Products.FirstOrDefault(p => p.Id == productId); // to zwraca null jak nie ma obiektu o danym ID
        }

        public static void Update(Product updatedProduct, ProductDbContext productDbContext)
        {
            if (updatedProduct == null)
                throw new ArgumentNullException(nameof(updatedProduct));

            if (updatedProduct.Id <= 0)
                throw new ArgumentException("Invalid product ID", nameof(updatedProduct.Id));

            var existingProduct = productDbContext.Products.FirstOrDefault(p => p.Id == updatedProduct.Id);

            if (existingProduct == null)
                throw new InvalidOperationException($"Product with ID {updatedProduct.Id} not found.");

            existingProduct.Name = updatedProduct.Name;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.Weight = updatedProduct.Weight;
            existingProduct.Description = updatedProduct.Description;

            productDbContext.SaveChanges();
        }

        public static void Delete(int productId, ProductDbContext productDbContext)
        {
            if (productId <= 0)
                throw new ArgumentException("Invalid product ID", nameof(productId));

            var product = productDbContext.Products.FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                throw new InvalidOperationException($"Product with ID {productId} not found.");
            }

            productDbContext.Products.Remove(product);
            productDbContext.SaveChanges();
        }



    }
}