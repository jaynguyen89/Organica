using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class StaffRole
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public int RoleId { get; set; }
        public string WorkDescription { get; set; }

        public virtual StoreRole Role { get; set; }
        public virtual StoreStaff Staff { get; set; }
    }
}
