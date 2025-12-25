using EthernetShop.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthernetShop.Services
{
    public class ProductService
    {
        private readonly EthernetShopContext _db = DBService.Instance.Context;
        public ObservableCollection<Product> Products { get; set; } = new();
        public void GetAll()
        {
            var products = _db.Products.ToList();
            Products.Clear();
            foreach (var product in products)
                Products.Add(product);
        }
        public int Commit() => _db.SaveChanges();
        public ProductService()
        {
            GetAll();
        }
        public void Add(Product product)
        {
            var _product = new Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Rating = product.Rating,
                CreatedAt = product.CreatedAt,
                Category = product.Category,
                CategoryId = product.CategoryId,
                Brand = product.Brand,
                BrandId = product.BrandId,

            };
            _db.Add<Product>(_product);
            Commit();
            Products.Add(_product);
        }
        public void Remove(Product product)
        {
            _db.Remove<Product>(product);
            if (Commit() > 0)
                if (Products.Contains(product))
                    Products.Remove(product);
        }

        public void LoadRelation(Product product, string relation)
        {
            var entry = _db.Entry(product);
            var navigation = entry.Metadata.FindNavigation(relation)
            ?? throw new InvalidOperationException($"Navigation '{relation}' not found");

            if (navigation.IsCollection)
            {
                entry.Collection(relation).Load();
            }
            else
            {
                entry.Reference(relation).Load();
            }
        }
    }
}
