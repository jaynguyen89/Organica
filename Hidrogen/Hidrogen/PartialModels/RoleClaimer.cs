namespace Hidrogen.Models {

    public partial class RoleClaimer {

        public bool AllowCreate { get; set; }
        public bool AllowView { get; set; }
        public bool AllowEditOwn { get; set; }
        public bool AllowEditOthers { get; set; }
        public bool AllowDeleteOwn { get; set; }
        public bool AllowDeleteOthers { get; set; }
        public bool AllowReviveOwn { get; set; }
        public bool AllowReviveOthers { get; set; }
    }
}
