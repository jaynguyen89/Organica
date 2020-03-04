using HelperLibrary.ViewModels;
using System.Threading.Tasks;

namespace HelperLibrary.Interfaces {

    public interface IGoogleReCaptchaService {

        Task<ReCaptchaVerification> IsHumanRegistration(string captchaToken = null);
    }
}
