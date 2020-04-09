namespace Hidrogen.Models
{
    public class RoleClaimer
    {
        public int Id { get; set; }
        public int HidrogenianId { get; set; }
        public int RoleId { get; set; }
        public bool AllowCreate { get; set; }
        public bool AllowView { get; set; }
        public bool AllowEditOwn { get; set; }
        public bool AllowEditOthers { get; set; }
        public bool AllowDeleteOwn { get; set; }
        public bool AllowDeleteOthers { get; set; }
        public bool AllowReviveOwn { get; set; }
        public bool AllowReviveOthers { get; set; }
        public string AllowTemporarily { get; set; }

        public virtual Hidrogenian Hidrogenian { get; set; }
        public virtual HidroRole Role { get; set; }
    }
}
