using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class Item
    {
        public Item()
        {
            ItemAsset = new HashSet<ItemAsset>();
            ItemBundle = new HashSet<ItemBundle>();
            ItemDetail = new HashSet<ItemDetail>();
            ItemStock = new HashSet<ItemStock>();
            ItemVariation = new HashSet<ItemVariation>();
            Warranty = new HashSet<Warranty>();
        }

        public int Id { get; set; }
        public int ListingId { get; set; }
        public decimal ItemPrice { get; set; }
        public string ItemName { get; set; }

        public virtual Listing Listing { get; set; }
        public virtual ICollection<ItemAsset> ItemAsset { get; set; }
        public virtual ICollection<ItemBundle> ItemBundle { get; set; }
        public virtual ICollection<ItemDetail> ItemDetail { get; set; }
        public virtual ICollection<ItemStock> ItemStock { get; set; }
        public virtual ICollection<ItemVariation> ItemVariation { get; set; }
        public virtual ICollection<Warranty> Warranty { get; set; }
    }
}
