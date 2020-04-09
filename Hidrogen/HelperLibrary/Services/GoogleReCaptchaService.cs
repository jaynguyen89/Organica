using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HelperLibrary.Common;
using HelperLibrary.Interfaces;
using HelperLibrary.ViewModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HelperLibrary.Services {

    public class GoogleReCaptchaService : IGoogleReCaptchaService {

        private readonly ILogger<GoogleReCaptchaService> _logger;
        private readonly HttpClient GoogleReCaptchaRequest = new HttpClient();

        public GoogleReCaptchaService(
            ILogger<GoogleReCaptchaService> logger
        ) {
            _logger = logger;

            GoogleReCaptchaRequest.BaseAddress = new Uri(HidroConstants.GOOGLE_CAPTCHA_ENDPOINT);

            GoogleReCaptchaRequest.DefaultRequestHeaders.Accept.Clear();
            GoogleReCaptchaRequest.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ReCaptchaVerification> IsHumanRegistration(string captchaToken = null) {
            _logger.LogInformation("HidrogenianController.IsHumanRegistration - Checking starts.");

            if (captchaToken == null)
                return new ReCaptchaVerification { Result = false };

            var response = await GoogleReCaptchaRequest.PostAsJsonAsync(
                "?secret=" + HidroConstants.GOOGLE_CAPTCHA_SECRET_KEY + "&response=" + captchaToken,
                HttpCompletionOption.ResponseContentRead
            );

            var verified = new ReCaptchaVerification();
            if (response.IsSuccessStatusCode)
                verified = JsonConvert.DeserializeObject<ReCaptchaVerification>(await response.Content.ReadAsStringAsync());

            return verified;
        }
    }
}
