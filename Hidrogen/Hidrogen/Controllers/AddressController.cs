using System.Collections.Generic;
using System.Threading.Tasks;
using Hidrogen.Attributes;
using Hidrogen.Services;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels;
using Hidrogen.ViewModels.Address;
using Hidrogen.ViewModels.Address.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static HelperLibrary.HidroEnums;

namespace Hidrogen.Controllers {

    [ApiController]
    [Route("address")]
    public class AddressController {
        
        private readonly ILogger<AddressController> _logger;
        private readonly IHidroAddressService _addressService;

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

        [HttpPost("add-address")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("1,0,0,0,0,0,0,0")]
        public async Task<JsonResult> AddNewAddressFor(AddressBinderVM binder) {
            _logger.LogInformation("AddressController.GetAddressListFor - hidrogenianId=" + binder.HidrogenianId);

            IGenericAddressVM address;
            if (binder.LocalAddress == null) address = binder.StandardAddress;
            else address = binder.LocalAddress;

            var verification = VerifyAddress(address);
            verification.AddRange(address.VerifyTitle());
            if (verification.Count != 0) {
                var messages = binder.LocalAddress == null ? address.sAddress.GenerateErrorMessages(verification)
                                                                      : address.lAddress.GenerateErrorMessages(verification);
                return new JsonResult(new { Result = RESULTS.FAILED, Message = messages });
            }

            var rawAddress = await _addressService.InsertRawAddressFor(binder.HidrogenianId, address);

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
        public async Task<JsonResult> UpdateHidrogenianAddress(AddressBinderVM binder) {
            IGenericAddressVM address;
            if (binder.LocalAddress == null) address = binder.StandardAddress;
            else address = binder.LocalAddress;
            
            _logger.LogInformation("AddressController.UpdateHidrogenianAddress - addressId=" + address.Id);

            var verification = VerifyAddress(address);
            verification.AddRange(address.VerifyTitle());
            if (verification.Count != 0) {
                var messages = binder.LocalAddress == null ? address.sAddress.GenerateErrorMessages(verification)
                                                                      : address.lAddress.GenerateErrorMessages(verification);
                return new JsonResult(new { Result = RESULTS.FAILED, Message = messages });
            }

            var updateResult = await _addressService.UpdateHidroAddress(address);

            if (!updateResult.Key.HasValue)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "No address found with the given data. Please try again." });

            if (!updateResult.Key.Value)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "Error occurred while attempting to update the address details. Please try again." });

            if (updateResult.Value == null)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "Error occurred while saving your new address details. Please try again." });

            return new JsonResult(new { Result = RESULTS.SUCCESS, Message = updateResult.Value });
        }
        
        [HttpPut("set-address-field")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,0,1,0,0,0,0,0")]
        public async Task<JsonResult> SetAddressAsPrimaryOrDeliveryFor(AddressSetterVM data) {
            _logger.LogInformation("AddressController.SetAddressAsPrimaryFor - Service starts.");

            var result = await _addressService.SetFieldDataForAddress(data);
            
            return !result.HasValue ? new JsonResult(new { Result = RESULTS.FAILED, Message = "Unable to find the address with given data. Please check again." })
                                    : (result.Value ? new JsonResult(new { Result = RESULTS.SUCCESS })
                                                    : new JsonResult(new { Result = RESULTS.FAILED, Message = "An error occurred while attempting to update the address. Please try again." }));
        }

        private List<int> VerifyAddress(IGenericAddressVM address) {
            var errors = new List<int>();
            var location = address.IsStandard ? address.sAddress : (GenericLocationVM)address.lAddress;

            errors.AddRange(location.VerifyBuildingName());
            errors.AddRange(location.VerifyStreetAddress());
            errors.AddRange(location.VerifyAltAddress());

            if (address.IsStandard) {
                var sAddress = address.sAddress;
                errors.AddRange(sAddress.VerifySuburb());
                errors.AddRange(sAddress.VerifyPostcode());
                errors.AddRange(sAddress.VerifyState());
            }
            else {
                var lAddress = address.lAddress;
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
