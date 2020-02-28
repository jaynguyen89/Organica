using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class RoleClaimer
    {
        public int Id { get; set; }
        public int HidrogenianId { get; set; }
        public int RoleId { get; set; }

        public virtual Hidrogenian Hidrogenian { get; set; }
        public virtual HidroRole Role { get; set; }
    }
}
