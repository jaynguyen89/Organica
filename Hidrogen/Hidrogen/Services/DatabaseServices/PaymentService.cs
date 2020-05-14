using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelperLibrary;
using HelperLibrary.Common;
using Hidrogen.Controllers;
using Hidrogen.DbContexts;
using Hidrogen.Models;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels.Payment;
using MethaneLibrary.Interfaces;
using MethaneLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static HelperLibrary.HidroEnums;

namespace Hidrogen.Services.DatabaseServices {

    public class PaymentService : IPaymentService {

        private readonly ILogger<PaymentService> _logger;
        private readonly IRuntimeLogService _runtimeLogger;
        private readonly HidrogenDbContext _dbContext;

        public PaymentService (
            ILogger<PaymentService> logger,
            IRuntimeLogService runtimeLogger,
            HidrogenDbContext dbContext
        ) {
            _logger = logger;
            _runtimeLogger = runtimeLogger;
            _dbContext = dbContext;
        }

        public async Task<PaymentDetailVM> AddBalanceToAccount(PaymentDetailVM paymentDetail) {
            _logger.LogInformation("PaymentService.AddAccountBalance - paymentMethodId=" + paymentDetail.PaymentMethod.Id);
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(PaymentService),
                Action = nameof(AddBalanceToAccount),
                Briefing = "Query database to add new balance to current balance for an account.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var dbPaymentMethod = await _dbContext.PaymentMethod.FirstOrDefaultAsync(p => p.HidrogenianId == paymentDetail.HidrogenianId);
            if (dbPaymentMethod == null) {
                dbPaymentMethod = new PaymentMethod {
                    HidrogenianId = paymentDetail.HidrogenianId,
                    AccountBalance = paymentDetail.PaymentMethod.AccountBalance,
                    BalanceAddedOn = DateTime.UtcNow
                };

                await _dbContext.PaymentMethod.AddAsync(dbPaymentMethod);
            }
            else {
                dbPaymentMethod.AccountBalance = paymentDetail.PaymentMethod.AccountBalance;
                dbPaymentMethod.BalanceAddedOn = DateTime.UtcNow;

                _dbContext.PaymentMethod.Update(dbPaymentMethod);
            }

            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("PaymentService.AddAccountBalance - Error: " + e);
                await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                    Controller = nameof(PaymentService),
                    Action = nameof(AddBalanceToAccount),
                    Briefing = "Exception occurred while saving changes: " + e,
                    Severity = LOGGING.ERROR.GetValue()
                });
                
                return null;
            }

            paymentDetail.PaymentMethod.BalanceDate = dbPaymentMethod.BalanceAddedOn.Value.ToString(DATE_FORMATS.FULL_DATE_FRIENDLY.GetValue());
            return paymentDetail;
        }

        public async Task<bool?> DeletePaymentMethodFor(int hidrogenianId, string deletedMethod) {
            _logger.LogInformation("PaymentService.DeletePaymentMethodFor - hidrogenianId=" + hidrogenianId + " deletedMethod=" + deletedMethod);
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(PaymentService),
                Action = nameof(DeletePaymentMethodFor),
                Briefing = "Query database to remove a payment method for an account.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var paymentMethod = await _dbContext.PaymentMethod.FirstOrDefaultAsync(p => p.HidrogenianId == hidrogenianId);
            if (paymentMethod == null) return null;

            if (deletedMethod == PAYMENT_METHODS.CARD.GetValue()) paymentMethod.RemoveCard();
            else paymentMethod.RemovePaypal();

            if (paymentMethod.AccountBalance == HidroConstants.EMPTY)
                _dbContext.PaymentMethod.Remove(paymentMethod);
            else
                _dbContext.PaymentMethod.Update(paymentMethod);

            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("PaymentService.DeletePaymentMethodFor - Error: " + e);
                await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                    Controller = nameof(PaymentService),
                    Action = nameof(DeletePaymentMethodFor),
                    Briefing = "Exception occurred while removing: " + e,
                    Severity = LOGGING.ERROR.GetValue()
                });
                
                return false;
            }

            return true;
        }

        public async Task<PaymentDetailVM> InsertNewPaymentMethod(PaymentDetailVM paymentDetail) {
            _logger.LogInformation("PaymentService.InsertNewPaymentMethod - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(PaymentService),
                Action = nameof(InsertNewPaymentMethod),
                Briefing = "Query database to save a payment method for an account.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var dbPaymentMethod = new PaymentMethod();
            if (paymentDetail.PaymentMethod.CreditCard != null)
                dbPaymentMethod = new PaymentMethod {
                    HolderName = paymentDetail.PaymentMethod.CreditCard.HolderName,
                    CardNumber = paymentDetail.PaymentMethod.CreditCard.CardNumber,
                    ExpiryDate = HelperProvider.ParseDateTimeString(paymentDetail.PaymentMethod.CreditCard.ExpiryDate, DATE_FORMATS.MONTH_YEAR.GetValue()),
                    SecurityCode = paymentDetail.PaymentMethod.CreditCard.SecurityCode,
                    CardAddedOn = DateTime.UtcNow
                };

            if (paymentDetail.PaymentMethod.Paypal != null)
                dbPaymentMethod = new PaymentMethod {
                    PaypalAddress = paymentDetail.PaymentMethod.Paypal.Email,
                    PaypalAddedOn = DateTime.UtcNow
                };

            dbPaymentMethod.HidrogenianId = paymentDetail.HidrogenianId;
            await _dbContext.PaymentMethod.AddAsync(dbPaymentMethod);

            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("PaymentService.InsertNewPaymentMethod - Error: " + e);
                await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                    Controller = nameof(PaymentService),
                    Action = nameof(InsertNewPaymentMethod),
                    Briefing = "Exception occurred while saving: " + e,
                    Severity = LOGGING.INFORMATION.GetValue()
                });
                return null;
            }

            paymentDetail.PaymentMethod.Id = dbPaymentMethod.Id;
            return paymentDetail;
        }

        public async Task<PaymentDetailVM> RetrievePaymentMethodsFor(int hidrogenianId) {
            _logger.LogInformation("PaymentService.RetrievePaymentMethodsFor - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(PaymentService),
                Action = nameof(RetrievePaymentMethodsFor),
                Briefing = "Query database to get all payment methods' details for hidrogenianId = " + hidrogenianId,
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var paymentMethod = await _dbContext.PaymentMethod.FirstOrDefaultAsync(p => p.HidrogenianId == hidrogenianId);
            return paymentMethod;
        }

        public async Task<KeyValuePair<bool, PaymentDetailVM>> UpdatePaymentMethods(PaymentDetailVM paymentDetail) {
            _logger.LogInformation("PaymentService.UpdatePaymentMethods - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(PaymentService),
                Action = nameof(UpdatePaymentMethods),
                Briefing = "Query database to update a payment method for an account.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var dbPaymentMethod = await _dbContext.PaymentMethod.FindAsync(paymentDetail.PaymentMethod.Id);
            if (dbPaymentMethod == null) return new KeyValuePair<bool, PaymentDetailVM>(false, null);

            dbPaymentMethod.HolderName = paymentDetail.PaymentMethod.CreditCard.HolderName;
            dbPaymentMethod.CardNumber = paymentDetail.PaymentMethod.CreditCard.CardNumber;
            dbPaymentMethod.ExpiryDate = HelperProvider.ParseDateTimeString(paymentDetail.PaymentMethod.CreditCard.ExpiryDate, DATE_FORMATS.MONTH_YEAR.GetValue());
            dbPaymentMethod.SecurityCode = paymentDetail.PaymentMethod.CreditCard.SecurityCode;
            dbPaymentMethod.CardAddedOn = DateTime.UtcNow;

            dbPaymentMethod.PaypalAddress = paymentDetail.PaymentMethod.Paypal.Email;
            dbPaymentMethod.PaypalAddedOn = DateTime.UtcNow;

            _dbContext.PaymentMethod.Update(dbPaymentMethod);
            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("PaymentService.UpdatePaymentMethods - Error: " + e);
                await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                    Controller = nameof(PaymentService),
                    Action = nameof(UpdatePaymentMethods),
                    Briefing = "Exception occurred while saving changes: " + e,
                    Severity = LOGGING.INFORMATION.GetValue()
                });
                
                return new KeyValuePair<bool, PaymentDetailVM>(true, null);
            }

            return new KeyValuePair<bool, PaymentDetailVM>(true, paymentDetail);
        }
    }
}