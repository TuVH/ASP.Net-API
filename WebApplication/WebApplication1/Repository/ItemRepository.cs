using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Mdels;

namespace WebApplication1.Repository
{
    public class ItemRepository : IItemRepository
    {
        private readonly List<Item> items = new()
        {
            new Item
            {
                Id = Guid.NewGuid(),
                Name = "Dong ho",
                Price = 50000,
                CreatedTime = DateTimeOffset.UtcNow
            },
            new Item
            {
                Id = Guid.NewGuid(),
                Name = "Dong ho1",
                Price = 60000,
                CreatedTime = DateTimeOffset.UtcNow
            },
            new Item
            {
                Id = Guid.NewGuid(),
                Name = "Dong ho2",
                Price = 70000,
                CreatedTime = DateTimeOffset.UtcNow
            },

        };

        public void CreateItems(Item item)
        {
            items.Add(item);
        }

        public void DeletedItems(Guid id)
        {
            var index = items.FindIndex(x => x.Id == id);
            items.RemoveAt(index);

        }

        public IEnumerable<Item> GetItems()
        {
            return items;
        }
        public Item GetItems(Guid id)
        {
            return items.Where(x => x.Id == id).FirstOrDefault();
        }

        public void UpdateItems(Item item)
        {
            var index = items.FindIndex(x => x.Id == item.Id);
            items[index] = item;
        }
    }
}
