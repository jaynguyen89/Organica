using System.Threading.Tasks;
using HelperLibrary;
using Hidrogen.Attributes;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels.Payment;
using MethaneLibrary.Interfaces;
using MethaneLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static HelperLibrary.HidroEnums;

namespace Hidrogen.Controllers {

    [ApiController]
    [Route("payment")]
    public class PaymentController : AppController {
        
        private readonly ILogger<PaymentController> _logger;
        private readonly IRuntimeLogService _runtimeLogger;
        private readonly IPaymentService _paymentService;

        public PaymentController(
            ILogger<PaymentController> logger,
            IRuntimeLogService runtimeLogger,
            IPaymentService paymentService,
            IDistributedCache redisCache
        ) : base(redisCache) {
            _logger = logger;
            _runtimeLogger = runtimeLogger;
            _paymentService = paymentService;
        }

        [HttpGet("payment-details/{hidrogenianId}")]
        [HidroActionFilter(ROLES.CUSTOMER)]
        [HidroAuthorize(PERMISSIONS.VIEW)]
        public async Task<JsonResult> GetPaymentDetailsFor(int hidrogenianId) {
            _logger.LogInformation("PaymentController.GetPaymentDetailsFor - hidrogenianId=" + hidrogenianId);
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(PaymentController),
                Action = nameof(GetPaymentDetailsFor),
                Briefing = "Get all payment details for user having hidrogenianId = " + hidrogenianId,
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var paymentDetails = await ReadFromRedisCacheAsync<PaymentDetailVM>("Account_PaymentDetails");
            if (paymentDetails != null) return new JsonResult(new {Result = RESULTS.SUCCESS, Message = paymentDetails});

            paymentDetails = await _paymentService.RetrievePaymentMethodsFor(hidrogenianId) ?? new PaymentDetailVM { HidrogenianId = hidrogenianId };

            await InsertRedisCacheEntryAsync("Account_PaymentDetails", paymentDetails);
            return new JsonResult(new {Result = RESULTS.SUCCESS, Message = paymentDetails});
        }

        [HttpPut("update-payment-details")]
        [HidroActionFilter(ROLES.CUSTOMER)]
        [HidroAuthorize(PERMISSIONS.EDIT_OWN)]
        public async Task<JsonResult> UpdatePaymentDetails(PaymentDetailVM paymentDetail) {
            _logger.LogInformation("PaymentController.UpdatePaymentDetails - Service starts.");

            var clone = paymentDetail.Copy();
            if (clone.PaymentMethod.CreditCard != null) {
                clone.PaymentMethod.CreditCard.CardNumber = null;
                clone.PaymentMethod.CreditCard.SecurityCode = null;
            }

            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(PaymentController),
                Action = nameof(UpdatePaymentDetails),
                Data = JsonConvert.SerializeObject(clone),
                Briefing = "Update payment methods in database for a user.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var (paymentMethodFound, updatedResult) = await _paymentService.UpdatePaymentMethods(paymentDetail);
            if (!paymentMethodFound)
                return new JsonResult(new {Result = RESULTS.FAILED, Message = "Unable to find a Payment Method associated with your account."});
            if (updatedResult == null)
                return new JsonResult(new {Result = RESULTS.FAILED, Message = "An error occurred while attempting to update your Payment Method. Please try again."});

            await RemoveRedisCacheEntryAsync("Account_PaymentDetails");
            return new JsonResult(new { Result = RESULTS.SUCCESS, Message = updatedResult });
        }

        [HttpGet("add-payment-details")]
        [HidroActionFilter(ROLES.CUSTOMER)]
        [HidroAuthorize(PERMISSIONS.CREATE)]
        public async Task<JsonResult> AddPaymentDetails(PaymentDetailVM paymentDetail) {
            _logger.LogInformation("PaymentController.AddPaymentDetails - Service starts.");
            
            var clone = paymentDetail.Copy();
            if (clone.PaymentMethod.CreditCard != null) {
                clone.PaymentMethod.CreditCard.CardNumber = null;
                clone.PaymentMethod.CreditCard.SecurityCode = null;
            }

            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(PaymentController),
                Action = nameof(AddPaymentDetails),
                Data = JsonConvert.SerializeObject(clone),
                Briefing = "Add new payment method into database for a user.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var newPaymentMethod = await _paymentService.InsertNewPaymentMethod(paymentDetail);
            if (newPaymentMethod == null)
                return new JsonResult(new {Result = RESULTS.FAILED, Message = "An error occurred while saving your new Payment Method. Please try again."});
            
            await RemoveRedisCacheEntryAsync("Account_PaymentDetails");
            return new JsonResult(new { Result = RESULTS.SUCCESS, Message = newPaymentMethod });
        }

        [HttpDelete("remove-payment-details/{deletedMethod?}")]
        [HidroActionFilter(ROLES.CUSTOMER)]
        [HidroAuthorize(PERMISSIONS.DELETE_OWN)]
        public async Task<JsonResult> RemovePaymentDetailsFor(int hidrogenianId, string deletedMethod = "card") {
            _logger.LogInformation("PaymentController.RemovePaymentDetailsFor - hidrogenianId=" + hidrogenianId);
            
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(PaymentController),
                Action = nameof(RemovePaymentDetailsFor),
                Briefing = "Remove a payment method for user having hidrogenianId = " + hidrogenianId + " and deletedMethod = " + deletedMethod,
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var removed = await _paymentService.DeletePaymentMethodFor(hidrogenianId, deletedMethod);
            if (!removed.HasValue) return new JsonResult(new {Result = RESULTS.FAILED, Message = "Unable to find a Payment Method associated with your account."});
            if (!removed.Value) return new JsonResult(new { Result = RESULTS.FAILED, Message = "An error occurred while attempting to remove your Payment Method. Please try again." });
            
            await RemoveRedisCacheEntryAsync("Account_PaymentDetails");
            return new JsonResult(new { Result = RESULTS.SUCCESS });
        }

        public async Task<JsonResult> AddBalanceToAccount(PaymentDetailVM paymentDetail) {
            _logger.LogInformation("PaymentController.AddBalanceToAccount - Service starts.");
            
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(PaymentController),
                Action = nameof(RemovePaymentDetailsFor),
                Data = JsonConvert.SerializeObject(paymentDetail),
                Briefing = "Add more account balance for a user.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var added = await _paymentService.AddBalanceToAccount(paymentDetail);
            if (added == null) return new JsonResult(new {Result = RESULTS.FAILED, Message = "An error occurred while saving your account balance. Please try again."});
            
            await RemoveRedisCacheEntryAsync("Account_PaymentDetails");
            return new JsonResult(new { Result = RESULTS.SUCCESS });
        }
    }
}