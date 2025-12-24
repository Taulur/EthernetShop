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
    public class TagService
    {
        private readonly EthernetShopContext _db = DBService.Instance.Context;
        public ObservableCollection<Tag> Tags { get; set; } = new();
        public void GetAll()
        {
            var tags = _db.Tags.ToList();
            Tags.Clear();
            foreach (var category in tags)
                Tags.Add(category);
        }
        public int Commit() => _db.SaveChanges();
        public TagService()
        {
            GetAll();
        }
        public void Add(Tag tag)
        {
            var _tag = new Tag
            {
                Name = tag.Name,
            };
            _db.Add<Tag>(_tag);
            Commit();
            Tags.Add(_tag);
        }
        public void Remove(Tag tag)
        {
            _db.Remove<Tag>(tag);
            if (Commit() > 0)
                if (Tags.Contains(tag))
                    Tags.Remove(tag);
        }

        public void LoadRelation(Tag tag, string relation)
        {
            var entry = _db.Entry(tag);
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
