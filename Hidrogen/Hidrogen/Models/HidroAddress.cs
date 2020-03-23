using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class HidroAddress
    {
        public int Id { get; set; }
        public int HidrogenianId { get; set; }
        public int? LocationId { get; set; }
        public bool IsRefined { get; set; }
        public bool IsPrimaryAddress { get; set; }
        public bool IsDeliveryAddress { get; set; }

        public virtual Hidrogenian Hidrogenian { get; set; }
    }
}
