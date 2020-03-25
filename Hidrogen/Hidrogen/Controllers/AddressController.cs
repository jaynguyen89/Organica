using Hidrogen.Attributes;
using Hidrogen.Services;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels.Address.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
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

            var updateResult = await _addressService.UpdateHidroAddress(address);

            if (!updateResult.Key.HasValue)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "No address found with the given data. Please try again." });

            if (!updateResult.Key.Value)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "Error occurred while attempting to update the address details. Please try again." });

            if (updateResult.Value == null)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "Error occurred while saving your new address details. Please try agian." });

            return new JsonResult(new { Result = RESULTS.SUCCESS, Message = updateResult.Value });
        }
    }
}
