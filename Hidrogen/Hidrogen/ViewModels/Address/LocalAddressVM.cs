using System.Collections.Generic;
using System.Text.RegularExpressions;
using HelperLibrary;
using HelperLibrary.Common;
using Hidrogen.Models;
using Hidrogen.ViewModels.Address.Generic;
using Newtonsoft.Json;

namespace Hidrogen.ViewModels.Address {

    public class LocalAddressVM : IGenericAddressVM {

        public int Id { get; set; }
        
        public string Title { get; set; }

        public bool IsPrimary { get; set; }

        public bool ForDelivery { get; set; }

        public bool IsStandard { get; set; }

        public bool IsRefined { get; set; }
        
        public string LastUpdate { get; set; }

        [JsonProperty("lAddress")]
        public LocalLocationVM lAddress { get; set; }

        [JsonProperty("sAddress")]
        public StandardLocationVM sAddress { get; set; } //Always null

        public string NormalizedAddress => lAddress.ProduceNormalizedAddress();

        public static explicit operator LocalAddressVM(FineLocation location) {
            return new LocalAddressVM {
                IsStandard = location.IsStandard,
                lAddress = location
            };
        }

        public static explicit operator LocalAddressVM(RawLocation location) {
            return new LocalAddressVM {
                IsStandard = location.IsStandard,
                lAddress = location
            };
        }

        internal void SetAddressValues(HidroAddress address) {
            Id = address.Id;
            Title = address.Title;
            IsPrimary = address.IsPrimaryAddress;
            ForDelivery = address.IsDeliveryAddress;
            IsRefined = address.IsRefined;
            LastUpdate = address.LastUpdated
                .ToString(HidroEnums.DATE_FORMATS.NONSEC_DATETIME_FRIENDLY.GetValue());
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