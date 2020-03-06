using HelperLibrary.Common;

namespace Hidrogen.ViewModels {

    public class HidroProfileVM {

        public int Id { get; set; }

        public int HidrogenianId { get; set; }

        public string FamilyName { get; set; }

        public string GivenName { get; set; }

        public string FullName => GivenName + HidroConstants.WHITE_SPACE + FamilyName;
    }
}