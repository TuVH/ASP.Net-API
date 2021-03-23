using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Dtos
{
    public record CreatedItemDto
    {
        [Required]
        public string Name { get; init; }

        [Required]
        [Range(0,10000000)]
        public decimal Price { get; init; }
    }
}
