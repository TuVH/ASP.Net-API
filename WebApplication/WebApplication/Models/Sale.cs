using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication.Models
{
    public partial class Sale
    {
        public int SaleId { get; set; }
        public string StoreId { get; set; }
        public string OrderNum { get; set; }
        public DateTime OrderDate { get; set; }
        public short Quantity { get; set; }
        public string PayTerms { get; set; }
        public int BookId { get; set; }

        public virtual Book Book { get; set; }
        public virtual Store Store { get; set; }
    }
}
