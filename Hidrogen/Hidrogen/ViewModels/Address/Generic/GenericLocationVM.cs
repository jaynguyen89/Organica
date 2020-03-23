namespace Hidrogen.ViewModels.Address.Generic {

    public abstract class GenericLocationVM {

        public string BuildingName { get; set; }

        public string StreetAddress { get; set; }

        public string AlternateAddress { get; set; }

        public string Country { get; set; }

        public string Note { get; set; }

        public abstract string ProduceNormalizedAddress();
    }
}