using Hidrogen.ViewModels.Address;

namespace Hidrogen.ViewModels {
    
    public class AddressBinderVM {
        
        public int HidrogenianId { get; set; }
        
        public LocalAddressVM LocalAddress { get; set; }
        
        public StandardAddressVM StandardAddress { get; set; }
    }
}