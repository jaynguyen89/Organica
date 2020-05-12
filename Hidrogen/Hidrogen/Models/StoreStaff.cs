using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class StoreStaff
    {
        public StoreStaff()
        {
            StaffRole = new HashSet<StaffRole>();
        }

        public int Id { get; set; }
        public int StoreId { get; set; }
        public int StaffId { get; set; }
        public byte Position { get; set; }
        public string Description { get; set; }
        public DateTime AddedOn { get; set; }

        public virtual Hidrogenian Staff { get; set; }
        public virtual HidroStore Store { get; set; }
        public virtual ICollection<StaffRole> StaffRole { get; set; }
    }
}
