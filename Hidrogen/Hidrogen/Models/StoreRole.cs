using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class StoreRole
    {
        public StoreRole()
        {
            StaffRole = new HashSet<StaffRole>();
        }

        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<StaffRole> StaffRole { get; set; }
    }
}
