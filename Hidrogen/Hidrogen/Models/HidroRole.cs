using System.Collections.Generic;

namespace Hidrogen.Models
{
    public class HidroRole
    {
        public HidroRole()
        {
            RoleClaimer = new HashSet<RoleClaimer>();
        }

        public int Id { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }

        public virtual ICollection<RoleClaimer> RoleClaimer { get; set; }
    }
}
