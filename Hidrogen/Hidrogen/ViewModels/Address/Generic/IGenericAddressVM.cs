namespace Hidrogen.ViewModels.Address.Generic {

    public interface IGenericAddressVM {

        int Id { get; set; }

        bool IsPrimaryAddress { get; set; }

        bool IsDeliveryAddress { get; set; }

        bool IsStandard { get; set; }
    }
}