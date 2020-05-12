using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class ItemVariation
    {
        public ItemVariation()
        {
            ItemBasket = new HashSet<ItemBasket>();
            ItemStock = new HashSet<ItemStock>();
        }

        public int Id { get; set; }
        public int ItemId { get; set; }
        public string VariationName { get; set; }
        public string AvatarData { get; set; }
        public decimal? VariationPrice { get; set; }

        public virtual Item Item { get; set; }
        public virtual ICollection<ItemBasket> ItemBasket { get; set; }
        public virtual ICollection<ItemStock> ItemStock { get; set; }
    }
}
