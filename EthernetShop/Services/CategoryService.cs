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
    public class CategoryService
    {
        private readonly EthernetShopContext _db = DBService.Instance.Context;
        public ObservableCollection<Category> Categories { get; set; } = new();
        public void GetAll()
        {
            var categories = _db.Categories.ToList();
            Categories.Clear();
            foreach (var category in categories)
                Categories.Add(category);
        }
        public int Commit() => _db.SaveChanges();
        public CategoryService()
        {
            GetAll();
        }
        public void Add(Category category)
        {
            var _category = new Category
            {
                Name = category.Name,
            };
            _db.Add<Category>(_category);
            Commit();
            Categories.Add(_category);
        }
        public void Remove(Category category)
        {
            _db.Remove<Category>(category);
            if (Commit() > 0)
                if (Categories.Contains(category))
                    Categories.Remove(category);
        }

        public void LoadRelation(Category category, string relation)
        {
            var entry = _db.Entry(category);
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
