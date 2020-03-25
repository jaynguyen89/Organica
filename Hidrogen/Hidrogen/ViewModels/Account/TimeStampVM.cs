using HelperLibrary;
using HelperLibrary.Common;
using Hidrogen.Models;
using Newtonsoft.Json;
using static HelperLibrary.HidroEnums;

namespace Hidrogen.ViewModels.Account {

    public class TimeStampVM {

        public string RegisteredOn { get; set; }

        public string LastUpdate { get; set; }

        public string LastOnline { get; set; }

        public string LastOffline { get; set; }

        public string LastDevice { get; set; } = HidroConstants.NA;

        public string LastIpAddress { get; set; } = HidroConstants.NA;

        public string LastLocation { get; set; } = HidroConstants.NA;

        public string LastBrowser { get; set; } = HidroConstants.NA;

        public static implicit operator TimeStampVM(Hidrogenian account) {
            var timeStamps = string.IsNullOrEmpty(account.LastDeviceInfo) ? new TimeStampVM() : JsonConvert.DeserializeObject<TimeStampVM>(account.LastDeviceInfo);

            timeStamps.RegisteredOn = !account.CreatedOn.HasValue ? HidroConstants.NA : account.CreatedOn.Value.ToString(DATE_FORMATS.NONSEC_DATETIME_FRIENDLY.GetValue());
            timeStamps.LastUpdate = !account.UpdatedOn.HasValue ? HidroConstants.NA : account.UpdatedOn.Value.ToString(DATE_FORMATS.NONSEC_DATETIME_FRIENDLY.GetValue());
            timeStamps.LastOnline = !account.LastSignin.HasValue ? HidroConstants.NA : account.LastSignin.Value.ToString(DATE_FORMATS.NONSEC_DATETIME_FRIENDLY.GetValue());
            timeStamps.LastOffline = !account.LastSignout.HasValue ? HidroConstants.NA : account.LastSignout.Value.ToString(DATE_FORMATS.NONSEC_DATETIME_FRIENDLY.GetValue());

            return timeStamps;
        }
    }
}
