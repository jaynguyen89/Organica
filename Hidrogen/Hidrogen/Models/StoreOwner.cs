using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class StoreOwner
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int OwnerId { get; set; }
        public float SharedProfit { get; set; }
        public string OwnerNote { get; set; }
        public DateTime? JointOn { get; set; }

        public virtual Hidrogenian Owner { get; set; }
        public virtual HidroStore Store { get; set; }
    }
}
