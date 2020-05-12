using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class Order
    {
        public Order()
        {
            Trading = new HashSet<Trading>();
        }

        public int Id { get; set; }
        public int CartId { get; set; }
        public decimal OrderTotal { get; set; }
        public bool? IsFullyPaid { get; set; }
        public DateTime MadeOn { get; set; }

        public virtual ShoppingCart Cart { get; set; }
        public virtual ICollection<Trading> Trading { get; set; }
    }
}
