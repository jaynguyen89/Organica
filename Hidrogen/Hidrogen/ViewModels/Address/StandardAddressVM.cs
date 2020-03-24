using Hidrogen.Models;
using Hidrogen.ViewModels.Address.Generic;
using System;

namespace Hidrogen.ViewModels.Address {

    public class StandardAddressVM : IGenericAddressVM {
        
        public int Id { get; set; }

        public bool IsPrimary { get; set; }

        public bool ForDelivery { get; set; }

        public bool IsStandard { get; set; }

        public bool IsRefined { get; set; }

        public LocalLocationVM _lAddress { get; set; } //Always null

        public StandardLocationVM _sAddress { get; set; }

        public string NormalizedAddress => _sAddress.ProduceNormalizedAddress();

        public static explicit operator StandardAddressVM(FineLocation location) {
            return new StandardAddressVM {
                IsStandard = location.IsStandard,
                _sAddress = location
            };
        }

        public static explicit operator StandardAddressVM(RawLocation location) {
            return new StandardAddressVM {
                IsStandard = location.IsStandard,
                _sAddress = location
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