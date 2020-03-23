using Hidrogen.ViewModels.Address.Generic;

namespace Hidrogen.ViewModels.Address {

    public class StandardLocationVM : GenericLocationVM {

        public int Id { get; set; }

        public string Suburb { get; set; }

        public string Postcode { get; set; }

        public string State { get; set; }

        public override string ProduceNormalizedAddress() {
            throw new System.NotImplementedException();
        }
    }
}