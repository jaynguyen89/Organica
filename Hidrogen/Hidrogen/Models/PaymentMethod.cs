using System;

namespace Hidrogen.Models
{
    public partial class PaymentMethod
    {
        public int Id { get; set; }
        public int HidrogenianId { get; set; }
        public decimal? AccountBalance { get; set; }
        public DateTime? BalanceAddedOn { get; set; }
        public string HolderName { get; set; }
        public string CardNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string SecurityCode { get; set; }
        public DateTime? CardAddedOn { get; set; }
        public string PaypalAddress { get; set; }
        public DateTime? PaypalAddedOn { get; set; }

        public virtual Hidrogenian Hidrogenian { get; set; }
    }
}
