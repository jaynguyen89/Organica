using System.Collections.Generic;
using System.Threading.Tasks;
using Hidrogen.ViewModels.Address;
using Hidrogen.ViewModels.Address.Generic;

namespace Hidrogen.Services.Interfaces {

    public interface IHidroAddressService {

        /// <summary>
        /// Returns null if an error occurred during the process, otherwise, returns the list of addresses.
        /// </summary>
        Task<List<IGenericAddressVM>> RetrieveAddressesForHidrogenian(int hidrogenianId);

        /// <summary>
        /// Returns null on error, otherwise, an instance of IGenericAddressVM, which can be either StandardAddressVM or LocalAddressVM.
        /// </summary>
        Task<IGenericAddressVM> InsertRawAddressFor(int hidrogenianId, IGenericAddressVM address);
        
        /// <summary>
        /// Returns null if no address found with addressId, returns false if removal failed, otherwise, returns true.
        /// </summary>
        Task<KeyValuePair<bool, bool>?> RemoveHidroAddress(int addressId);
        
        /// <summary>
        /// Returns Key-Value pair. Key == null if no address found with address ID, Key == false if inserting rawLocation failed.
        /// When Key == true, Value == null if database updating address failed, otherwise, Value is the updated address.
        /// </summary>
        Task<KeyValuePair<bool?, IGenericAddressVM>> UpdateHidroAddress(IGenericAddressVM address);
        
        /// <summary>
        /// Returns null if no address found, false if database update failed, otherwise true.
        /// </summary>
        Task<bool?> SetFieldDataForAddress(AddressSetterVM data);
    }
}