using HelperLibrary.Interfaces;
using HelperLibrary.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelperLibrary.Services {

    public class EmailSenderService : IEmailSenderService {

        public Task<bool> SendEmail(EmailParamVM message)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> SendEmails(List<EmailParamVM> messages)
        {
            throw new System.NotImplementedException();
        }
    }
}
