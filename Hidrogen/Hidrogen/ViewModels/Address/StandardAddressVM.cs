using System.Collections.Generic;
using System.Text.RegularExpressions;
using HelperLibrary;
using HelperLibrary.Common;
using Hidrogen.Models;
using Hidrogen.ViewModels.Address.Generic;

namespace Hidrogen.ViewModels.Address {

    public class StandardAddressVM : IGenericAddressVM {
        
        public int Id { get; set; }
        
        public string Title { get; set; }

        public bool IsPrimary { get; set; }

        public bool ForDelivery { get; set; }

        public bool IsStandard { get; set; }

        public bool IsRefined { get; set; }
        
        public string LastUpdate { get; set; }

        public LocalLocationVM lAddress { get; set; } //Always null

        public StandardLocationVM sAddress { get; set; }

        public string NormalizedAddress => sAddress.ProduceNormalizedAddress();

        public static explicit operator StandardAddressVM(FineLocation location) {
            return new StandardAddressVM {
                IsStandard = location.IsStandard,
                sAddress = location
            };
        }

        public static explicit operator StandardAddressVM(RawLocation location) {
            return new StandardAddressVM {
                IsStandard = location.IsStandard,
                sAddress = location
            };
        }

        internal void SetAddressValues(HidroAddress address) {
            Id = address.Id;
            Title = address.Title;
            IsPrimary = address.IsPrimaryAddress;
            ForDelivery = address.IsDeliveryAddress;
            IsRefined = address.IsRefined;
            LastUpdate =  LastUpdate = address.LastUpdated
                .ToString(HidroEnums.DATE_FORMATS.NONSEC_DATETIME_FRIENDLY.GetValue());;
        }
        
        public List<int> VerifyTitle() {
            var errors = new List<int>();

            if (string.IsNullOrEmpty(Title) || string.IsNullOrWhiteSpace(Title)) {
                Title = null;
                return errors;
            }

            Title = Title.Trim().Replace(HidroConstants.DOUBLE_SPACE, HidroConstants.WHITE_SPACE);
            Title = HelperProvider.CapitalizeFirstLetterOfEachWord(Title);
            
            var lenTest = new Regex(@".{1,30}");
            if (!lenTest.IsMatch(Title))
                errors.Add(13);

            var rx = new Regex(@"^[A-Za-z'.\- ]*$");
            if (!rx.IsMatch(Title))
                errors.Add(14);

            return errors;
        }
    }
}