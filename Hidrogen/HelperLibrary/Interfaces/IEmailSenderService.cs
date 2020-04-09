using System.Collections.Generic;
using System.Threading.Tasks;
using HelperLibrary.ViewModels;

namespace HelperLibrary.Interfaces {

    public interface IEmailSenderService {

        Task<bool> SendEmail(EmailParamVM message);
        Task<bool> SendEmails(List<EmailParamVM> messages);
    }
}
