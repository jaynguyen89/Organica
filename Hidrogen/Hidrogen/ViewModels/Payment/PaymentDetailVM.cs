using HelperLibrary;
using HelperLibrary.Common;
using Hidrogen.Models;
using static HelperLibrary.HidroEnums;

namespace Hidrogen.ViewModels.Payment {

    public class PaymentDetailVM {

        public int HidrogenianId { get; set; }

        public PaymentMethodVM PaymentMethod { get; set; } = new PaymentMethodVM();

        public static implicit operator PaymentDetailVM(PaymentMethod detail) {
            return detail == null ? null : new PaymentDetailVM {
                HidrogenianId = detail.HidrogenianId,
                PaymentMethod = new PaymentMethodVM {
                    Id = detail.Id,
                    AccountBalance = detail.AccountBalance ?? HidroConstants.EMPTY,
                    BalanceDate = !detail.BalanceAddedOn.HasValue ? string.Empty : detail.BalanceAddedOn.Value.ToString(DATE_FORMATS.NONSEC_DATETIME_FRIENDLY.GetValue()),
                    CreditCard = new CreditCardVM {
                        HolderName = detail.HolderName,
                        CardNumber = detail.CardNumber,
                        ExpiryDate = !detail.ExpiryDate.HasValue ? string.Empty : detail.ExpiryDate.Value.ToString(DATE_FORMATS.MONTH_YEAR.GetValue()),
                        SecurityCode = detail.SecurityCode
                    },
                    Paypal = new PaypalVM {
                        Email = detail.PaypalAddress,
                        AddedOn = !detail.PaypalAddedOn.HasValue ? string.Empty : detail.PaypalAddedOn.Value.ToString(DATE_FORMATS.FULL_DATE_FRIENDLY.GetValue())
                    }
                }
            };
        }

        public bool IsThereABalance() {
            return !PaymentMethod.IsNoBalance();
        }

        /// <summary>
        /// Check if Credit Card was added. True if any, otherwise False.
        /// </summary>
        public bool IsThereACard() {
            return !PaymentMethod.CreditCard.IsNone();
        }

        /// <summary>
        /// Check if Paypal was added. True if any, otherwise False.
        /// </summary>
        public bool IsThereAPaypalAccount() {
            return !PaymentMethod.Paypal.IsNone();
        }
    }

    public class PaymentMethodVM {

        public int Id { get; set; }

        public decimal AccountBalance { get; set; }

        public string BalanceDate { get; set; }

        public CreditCardVM CreditCard { get; set; } = new CreditCardVM();

        public PaypalVM Paypal { get; set; } = new PaypalVM();

        public bool IsNoBalance() {
            return AccountBalance == HidroConstants.EMPTY;
        }
    }

    public class CreditCardVM {
        
        public string HolderName { get; set; }

        public string CardNumber { get; set; }

        public string ExpiryDate { get; set; }

        public string SecurityCode { get; set; }

        public bool IsNone() {
            return string.IsNullOrEmpty(HolderName) &&
                string.IsNullOrEmpty(CardNumber) &&
                string.IsNullOrEmpty(ExpiryDate) &&
                string.IsNullOrEmpty(SecurityCode);
        }
    }

    public class PaypalVM {

        public string Email { get; set; }

        public string AddedOn { get; set; }

        public bool IsNone() {
            return string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(AddedOn);
        }
    }
}