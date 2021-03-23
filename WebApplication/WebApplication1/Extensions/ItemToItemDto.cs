using WebApplication1.Dtos;
using WebApplication1.Mdels;

namespace WebApplication1.Extensions
{
    public static class ItemToItemDto
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                CreatedTime = item.CreatedTime
            };
        }
    }
}
