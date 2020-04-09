namespace Hidrogen.ViewModels.Authorization {

    public class RoleClaimerVM {

        public int Id { get; set; }

        public int HidrogenianId { get; set; }

        public HidroRoleVM Role { get; set; }
        
        public HidroPermissionVM Permissions { get; set; }
    }
}