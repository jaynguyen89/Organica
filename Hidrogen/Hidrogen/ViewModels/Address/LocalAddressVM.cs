using Hidrogen.Models;
using Hidrogen.ViewModels.Address.Generic;

namespace Hidrogen.ViewModels.Address {

    public class LocalAddressVM : IGenericAddressVM {

        public int Id { get; set; }

        public bool IsPrimary { get; set; }

        public bool ForDelivery { get; set; }

        public bool IsStandard { get; set; }

        public bool IsRefined { get; set; }

        public LocalLocationVM _lAddress { get; set; }

        public StandardLocationVM _sAddress { get; set; } //Always null

        public string NormalizedAddress => _lAddress.ProduceNormalizedAddress();

        public static explicit operator LocalAddressVM(FineLocation location) {
            return new LocalAddressVM {
                IsStandard = location.IsStandard,
                _lAddress = location
            };
        }

        public static explicit operator LocalAddressVM(RawLocation location) {
            return new LocalAddressVM {
                IsStandard = location.IsStandard,
                _lAddress = location
            };
        }

        internal void SetAddressValues(HidroAddress address) {
            Id = address.Id;
            IsPrimary = address.IsPrimaryAddress;
            ForDelivery = address.IsDeliveryAddress;
            IsRefined = address.IsRefined;
        }
    }
}