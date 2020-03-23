using Hidrogen.ViewModels.Address.Generic;

namespace Hidrogen.ViewModels.Address {

    public class StandardAddressVM : IGenericAddressVM {
        
        public int Id { get; set; }

        public bool IsPrimaryAddress { get; set; }

        public bool IsDeliveryAddress { get; set; }

        public bool IsStandard { get; set; }

        public StandardLocationVM Address { get; set; }

        public string NormalizedAddress => Address.ProduceNormalizedAddress();
    }
}