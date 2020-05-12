using System.Threading.Tasks;
using Hidrogen.Attributes;
using Hidrogen.Services;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels.Payment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static HelperLibrary.HidroEnums;

namespace Hidrogen.Controllers {

    [ApiController]
    [Route("payment")]
    public class PaymentController {
        
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentService _paymentService;

        public PaymentController(
            ILogger<PaymentController> logger,
            IPaymentService paymentService
        ) {
            _logger = logger;
            _paymentService = paymentService;
        }

        [HttpGet("payment-details/{hidrogenianId}")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,1,0,0,0,0,0,0")]
        public async Task<JsonResult> GetPaymentDetailsFor(int hidrogenianId) {
            _logger.LogInformation("PaymentController.GetPaymentDetailsFor - hidrogenianId=" + hidrogenianId);

            var details = await _paymentService.RetrievePaymentMethodsFor(hidrogenianId);
            if (details != null) return new JsonResult(new {Result = RESULTS.SUCCESS, Message = details});

            details = new PaymentDetailVM { HidrogenianId = hidrogenianId };
            return new JsonResult(new { Result = RESULTS.SUCCESS, Message = details });
        }

        [HttpPut("update-payment-details")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,0,1,0,0,0,0,0")]
        public async Task<JsonResult> UpdatePaymentDetails(PaymentDetailVM paymentDetail) {
            _logger.LogInformation("PaymentController.UpdatePaymentDetails - Service starts.");

            var (key, value) = await _paymentService.UpdatePaymentMethods(paymentDetail);

            return !key ? new JsonResult(new { Result = RESULTS.FAILED, Message = "Unable to find a Payment Method associated with your account." })
                                     : (value == null ? new JsonResult(new { Result = RESULTS.FAILED, Message = "An error occurred while attempting to update your Payment Method. Please try again." })
                                                                   : new JsonResult(new { Result = RESULTS.SUCCESS, Message = value }));
        }

        [HttpGet("add-payment-details")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("1,0,0,0,0,0,0,0")]
        public async Task<JsonResult> AddPaymentDetails(PaymentDetailVM paymentDetail) {
            _logger.LogInformation("PaymentController.AddPaymentDetails - Service starts.");

            var newPaymentMethod = await _paymentService.InsertNewPaymentMethod(paymentDetail);

            return newPaymentMethod == null ? new JsonResult(new { Result = RESULTS.FAILED, Message = "An error occurred while saving your new Payment Method. Please try again." })
                                            : new JsonResult(new { Result = RESULTS.SUCCESS, Message = newPaymentMethod });
        }

        [HttpDelete("remove-payment-details/{deletedMethod?}")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,0,1,0,1,0,0,0")]
        public async Task<JsonResult> RemovePaymentDetailsFor(int hidrogenianId, string deletedMethod = "card") {
            _logger.LogInformation("PaymentController.RemovePaymentDetailsFor - hidrogenianId=" + hidrogenianId);

            var removed = await _paymentService.DeletePaymentMethodFor(hidrogenianId, deletedMethod);

            return !removed.HasValue ? new JsonResult(new { Result = RESULTS.FAILED, Message = "Unable to find a Payment Method associated with your account." })
                                     : (!removed.Value ? new JsonResult(new { Result = RESULTS.FAILED, Message = "An error occurred while attempting to remove your Payment Method. Please try again." })
                                                      : new JsonResult(new { Result = RESULTS.SUCCESS }));
        }

        public async Task<JsonResult> AddBalanceToAccount(PaymentDetailVM paymentDetail) {
            _logger.LogInformation("PaymentController.AddBalanceToAccount - Service starts.");

            var added = await _paymentService.AddBalanceToAccount(paymentDetail);

            return added == null ? new JsonResult(new { Result = RESULTS.FAILED, Message = "An error occurred while saving your account balance. Please try again." })
                                 : new JsonResult(new { Result = RESULTS.SUCCESS });
        }
    }
}