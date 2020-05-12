using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class Listing
    {
        public Listing()
        {
            Classification = new HashSet<Classification>();
            Item = new HashSet<Item>();
            ItemBasket = new HashSet<ItemBasket>();
        }

        public int Id { get; set; }
        public int SellerId { get; set; }
        public int? LocationId { get; set; }
        public bool IsCombo { get; set; }
        public string Title { get; set; }
        public string Caption { get; set; }
        public string Headline { get; set; }
        public string SellerNote { get; set; }
        public string Description { get; set; }
        public byte? SellingFormat { get; set; }
        public bool IsActive { get; set; }
        public bool DropOnStockless { get; set; }
        public DateTime? CreatedOn { get; set; }

        public virtual ICollection<Classification> Classification { get; set; }
        public virtual ICollection<Item> Item { get; set; }
        public virtual ICollection<ItemBasket> ItemBasket { get; set; }
    }
}
