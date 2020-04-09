using System.Collections.Generic;
using Hidrogen.ViewModels.Address.Generic;

namespace Hidrogen.ViewModels.Address {

    public class HidroAddressVM {

        public int HidrogenianId { get; set; }

        public List<IGenericAddressVM> Addresses { get; set; }
    }
}