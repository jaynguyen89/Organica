using System.Collections.Generic;
using System.Threading.Tasks;
using Hidrogen.Attributes;
using Hidrogen.Services;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels.Address;
using Hidrogen.ViewModels.Address.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static HelperLibrary.HidroEnums;

namespace Hidrogen.Controllers {

    [ApiController]
    [Route("address")]
    public class AddressController {

        public readonly ILogger<AddressController> _logger;
        public readonly IHidroAddressService _addressService;

        public AddressController(
            ILogger<AddressController> logger,
            IHidroAddressService addressService
        ) {
            _logger = logger;
            _addressService = addressService;
        }

        [HttpGet("address-list/{hidrogenianId}")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,1,0,0,0,0,0,0")]
        public async Task<JsonResult> GetAddressListFor(int hidrogenianId) {
            _logger.LogInformation("AddressController.GetAddressListFor - hidrogenianId=" + hidrogenianId);

            var addressList = await _addressService.RetrieveAddressesForHidrogenian(hidrogenianId);

            return addressList == null ? new JsonResult(new { Result = RESULTS.FAILED, Message = "An error occurred while retrieving data for location list. Please reload page to try again." })
                                       : new JsonResult(new { Result = RESULTS.SUCCESS, Message = addressList });
        }

        [HttpPost("add-address/{hidrogenianId}")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("1,0,0,0,0,0,0,0")]
        public async Task<JsonResult> AddNewAddressFor(
            [FromQuery] int hidrogenianId,
            [FromBody] IGenericAddressVM address
        ) {
            _logger.LogInformation("AddressController.GetAddressListFor - hidrogenianId=" + hidrogenianId);

            var verification = VerifyAddress(address);
            if (verification.Count != 0) {
                var messages = address._lAddress.GenerateErrorMessages(verification);
                return new JsonResult(new { Result = RESULTS.FAILED, Message = messages });
            }

            var rawAddress = await _addressService.InsertRawAddressFor(hidrogenianId, address);

            return rawAddress == null ? new JsonResult(new { Result = RESULTS.FAILED, Message = "An error occurred while saving your address. Please try again." })
                                      : new JsonResult(new { Result = RESULTS.SUCCESS, Message = rawAddress });
        }

        [HttpDelete("remove-address/{addressId}")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,0,0,0,1,0,0,0")]
        public async Task<JsonResult> RemoveHidrogenianAddress(int addressId) {
            _logger.LogInformation("AddressController.RemoveHidrogenianAddress - addressId=" + addressId);

            var result = await _addressService.RemoveHidroAddress(addressId);

            return !result.HasValue ? new JsonResult(new { Result = RESULTS.FAILED, Message = "No address found with the given data. Please try again." })
                                    : (result.Value ? new JsonResult(new { Result = RESULTS.FAILED, Message = "Error occurred while attempting to delete the address. Please try again." })
                                                    : new JsonResult(new { Result = RESULTS.SUCCESS }));
        }

        [HttpPut("update-address")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,0,1,0,0,0,0,0")]
        public async Task<JsonResult> UpdateHidrogenianAddress(IGenericAddressVM address) {
            _logger.LogInformation("AddressController.UpdateHidrogenianAddress - addressId=" + address.Id);

            var verification = VerifyAddress(address);
            if (verification.Count != 0) {
                var messages = address._lAddress.GenerateErrorMessages(verification);
                return new JsonResult(new { Result = RESULTS.FAILED, Message = messages });
            }

            var updateResult = await _addressService.UpdateHidroAddress(address);

            if (!updateResult.Key.HasValue)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "No address found with the given data. Please try again." });

            if (!updateResult.Key.Value)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "Error occurred while attempting to update the address details. Please try again." });

            if (updateResult.Value == null)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "Error occurred while saving your new address details. Please try agian." });

            return new JsonResult(new { Result = RESULTS.SUCCESS, Message = updateResult.Value });
        }

        private List<int> VerifyAddress(IGenericAddressVM address) {
            var errors = new List<int>();
            var location = address.IsStandard ? address._sAddress : (GenericLocationVM)address._lAddress;

            errors.AddRange(location.VerifyBuildingName());
            errors.AddRange(location.VerifyStreetAddress());
            errors.AddRange(location.VerifyAltAddress());
            errors.AddRange(location.VerifyCountry());
            errors.AddRange(location.VerifyNote());

            StandardLocationVM sAddress;
            LocalLocationVM lAddress;

            if (address.IsStandard) {
                sAddress = address._sAddress;
                errors.AddRange(sAddress.VerifySuburb());
                errors.AddRange(sAddress.VerifyPostcode());
                errors.AddRange(sAddress.VerifyState());
            }
            else {
                lAddress = address._lAddress;
                errors.AddRange(lAddress.VerifyGroup());
                errors.AddRange(lAddress.VerifyLane());
                errors.AddRange(lAddress.VerifyQuarter());
                errors.AddRange(lAddress.VerifyHamlet());
                errors.AddRange(lAddress.VerifyCommute());
                errors.AddRange(lAddress.VerifyWard());
                errors.AddRange(lAddress.VerifyDistrict());
                errors.AddRange(lAddress.VerifyTown());
                errors.AddRange(lAddress.VerifyProvince());
                errors.AddRange(lAddress.VerifyCity());
            }

            return errors;
        }
    }
}
