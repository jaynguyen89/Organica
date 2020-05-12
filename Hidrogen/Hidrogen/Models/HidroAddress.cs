using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class HidroAddress
    {
        public HidroAddress()
        {
            ItemBasket = new HashSet<ItemBasket>();
        }

        public int Id { get; set; }
        public int HidrogenianId { get; set; }
        public int? LocationId { get; set; }
        public string Title { get; set; }
        public bool IsRefined { get; set; }
        public bool IsPrimaryAddress { get; set; }
        public bool IsDeliveryAddress { get; set; }
        public DateTime LastUpdated { get; set; }

        public virtual Hidrogenian Hidrogenian { get; set; }
        public virtual ICollection<ItemBasket> ItemBasket { get; set; }
    }
}
