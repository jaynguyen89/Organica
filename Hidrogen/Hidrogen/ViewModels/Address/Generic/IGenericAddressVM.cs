using System.Collections.Generic;

namespace Hidrogen.ViewModels.Address.Generic {

    public interface IGenericAddressVM {

        int Id { get; set; }
        
        string Title { get; set; }

        bool IsPrimary { get; set; }

        bool ForDelivery { get; set; }

        bool IsStandard { get; set; }

        bool IsRefined { get; set; }
        
        string LastUpdate { get; set; }

        LocalLocationVM lAddress { get; set; }

        StandardLocationVM sAddress { get; set; }

        string NormalizedAddress { get; }

        List<int> VerifyTitle();
    }
}