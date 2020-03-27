namespace Hidrogen.Models {

    public partial class PaymentMethod {

        public void RemoveCard() {
            HolderName = null;
            CardNumber = null;
            CardAddedOn = null;
            SecurityCode = null;
        }

        public void RemovePaypal() {
            PaypalAddress = null;
            PaypalAddedOn = null;
        }
    }
}
