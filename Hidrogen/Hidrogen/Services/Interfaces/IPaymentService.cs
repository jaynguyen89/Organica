using Hidrogen.ViewModels.Payment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hidrogen.Services.Interfaces {

    public interface IPaymentService {

        /// <summary>
        /// Returns an instance of PaymentDetailVM.
        /// </summary>
        Task<PaymentDetailVM> RetrievePaymentMethodsFor(int hidrogenianId);

        /// <summary>
        /// Returns Key-Value pair. Key == false if no payment method found by PaymentId. When Key == true,
        /// Value == null if database update failed, otherwise, Value holds an instance of PaymentDetailVM.
        /// </summary>
        Task<KeyValuePair<bool, PaymentDetailVM>> UpdatePaymentMethods(PaymentDetailVM paymentDetail);

        /// <summary>
        /// Returns null if database insertion failed, otherwise, an instance of PaymentDetailVM. 
        /// </summary>
        Task<PaymentDetailVM> InsertNewPaymentMethod(PaymentDetailVM paymentDetail);

        /// <summary>
        /// Returns null if no payment method found by hidrogenianId, false if database deletion failed, otherwise true.
        /// </summary>
        Task<bool?> DeletePaymentMethodFor(int hidrogenianId, string deletedMethod);

        /// <summary>
        /// Returns null if database update failed, otherwise an instance of PaymentDetailVM.
        /// </summary>
        Task<PaymentDetailVM> AddBalanceToAccount(PaymentDetailVM paymentDetail);
    }
}