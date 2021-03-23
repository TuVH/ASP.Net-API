using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Dtos;
using WebApplication1.Extensions;
using WebApplication1.Mdels;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;
        public ItemController(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }
        [HttpGet]
        public IEnumerable<ItemDto> GetItems()
        {
            var items = _itemRepository.GetItems().Select(x => x.AsDto());
            return items;
        }
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItems(Guid id)
        {
            var items = _itemRepository.GetItems(id);
            if (items == null)
            {
                return NotFound();
            }
            return items.AsDto();
        }

        [HttpPost]
        public ActionResult<ItemDto> CreatedItem(CreatedItemDto createdItem)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = createdItem.Name,
                Price = createdItem.Price,
                CreatedTime = DateTimeOffset.UtcNow
            };
            _itemRepository.CreateItems(item);


            //return item.AsDto();
            return CreatedAtAction(nameof(GetItems), new { id = item.Id }, item.AsDto());
        }
        [HttpPut("{id}")]
        public ActionResult CreatedItem(Guid id,UpdateItemDto updateItem)
        {
            var exist = _itemRepository.GetItems(id);
            if (exist == null)
            {
                return NotFound();
            }
            Item update = exist with
            {
                Name = updateItem.Name,
                Price = updateItem.Price
            };
            
            _itemRepository.UpdateItems(update);

            return NoContent();
        }
        [HttpDelete("{id}")]
        public ActionResult DeletedItem(Guid id)
        {
            var exist = _itemRepository.GetItems(id);
            if (exist == null)
            {
                return NotFound();
            }

            _itemRepository.DeletedItems(id);

            return NoContent();
        }
    }
}
