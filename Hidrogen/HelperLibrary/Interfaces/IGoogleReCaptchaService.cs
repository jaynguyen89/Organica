using System.Threading.Tasks;
using HelperLibrary.ViewModels;

namespace HelperLibrary.Interfaces {

    public interface IGoogleReCaptchaService {

        Task<ReCaptchaVerification> IsHumanRegistration(string captchaToken = null);
    }
}
