namespace Hidrogen.ViewModels.Address.Generic {

    public interface IGenericAddressVM {

        int Id { get; set; }

        bool IsPrimary { get; set; }

        bool ForDelivery { get; set; }

        bool IsStandard { get; set; }

        bool IsRefined { get; set; }

        LocalLocationVM _lAddress { get; set; }

        StandardLocationVM _sAddress { get; set; }

        string NormalizedAddress { get; }
    }
}