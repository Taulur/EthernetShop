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
    public class BrandService
    {
        private readonly EthernetShopContext _db = DBService.Instance.Context;
        public ObservableCollection<Brand> Brands { get; set; } = new();
        public void GetAll()
        {
            var brands = _db.Brands.ToList();
            Brands.Clear();
            foreach (var brand in brands)
                Brands.Add(brand);
        }
        public int Commit() => _db.SaveChanges();
        public BrandService()
        {
            GetAll();
        }
        public void Add(Brand brand)
        {
            var _brand = new Brand
            {
                Name = brand.Name,
            };
            _db.Add<Brand>(_brand);
            Commit();
            Brands.Add(_brand);
        }
        public void Remove(Brand brand)
        {
            _db.Remove<Brand>(brand);
            if (Commit() > 0)
                if (Brands.Contains(brand))
                    Brands.Remove(brand);
        }

        public void LoadRelation(Brand brand, string relation)
        {
            var entry = _db.Entry(brand);
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
