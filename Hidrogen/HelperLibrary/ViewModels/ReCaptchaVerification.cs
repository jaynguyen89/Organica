using System;
using Newtonsoft.Json;

namespace HelperLibrary.ViewModels {

    public class ReCaptchaVerification {

        [JsonProperty("success")]
        public bool Result { get; set; }

        [JsonProperty("challenge_ts")]
        public DateTime? VerifiedOn { get; set; }

        [JsonProperty("hostname")]
        public string HostName { get; set; }

        [JsonProperty("error-codes")]
        public string[] Errors { get; set; }
    }
}
