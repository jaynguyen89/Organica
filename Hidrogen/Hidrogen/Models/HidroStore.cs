using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class HidroStore
    {
        public HidroStore()
        {
            StoreOwner = new HashSet<StoreOwner>();
            StoreStaff = new HashSet<StoreStaff>();
        }

        public int Id { get; set; }
        public string StoreName { get; set; }
        public string StoreLogo { get; set; }
        public string Introduction { get; set; }
        public string CustomTheme { get; set; }
        public DateTime? OpenedOn { get; set; }

        public virtual ICollection<StoreOwner> StoreOwner { get; set; }
        public virtual ICollection<StoreStaff> StoreStaff { get; set; }
    }
}
