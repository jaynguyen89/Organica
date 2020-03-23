using Hidrogen.ViewModels.Address.Generic;

namespace Hidrogen.ViewModels.Address {

    public class LocalAddressVM : IGenericAddressVM {

        public int Id { get; set; }

        public bool IsPrimaryAddress { get; set; }

        public bool IsDeliveryAddress { get; set; }

        public bool IsStandard { get; set; }

        public LocalLocationVM Address { get; set; }

        public string NormalizedAddress => Address.ProduceNormalizedAddress();
    }
}