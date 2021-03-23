using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Mdels;

namespace WebApplication1.Repository
{
    public interface IItemRepository
    {
        IEnumerable<Item> GetItems();
        Item GetItems(Guid id);
        void CreateItems(Item item);

        void UpdateItems(Item item);

        void DeletedItems(Guid id);
    }
}
