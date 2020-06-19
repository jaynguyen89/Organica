using System.Collections.Generic;
using System.Threading.Tasks;
using HelperLibrary;
using Hidrogen.Attributes;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels;
using Hidrogen.ViewModels.Address;
using Hidrogen.ViewModels.Address.Generic;
using MethaneLibrary.Interfaces;
using MethaneLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static HelperLibrary.HidroEnums;

namespace Hidrogen.Controllers {

    [ApiController]
    [Route("address")]
    public class AddressController : AppController {
        
        private readonly ILogger<AddressController> _logger;
        private readonly IRuntimeLogService _runtimeLogger;
        private readonly IHidroAddressService _addressService;

        public AddressController(
            ILogger<AddressController> logger,
            IRuntimeLogService runtimeLogger,
            IHidroAddressService addressService,
            IDistributedCache redisCache
        ) : base(redisCache) {
            _logger = logger;
            _runtimeLogger = runtimeLogger;
            _addressService = addressService;
        }

        [HttpGet("address-list/{hidrogenianId}")]
        [HidroActionFilter(ROLES.CUSTOMER)]
        [HidroAuthorize(PERMISSIONS.VIEW)]
        public async Task<JsonResult> GetAddressListFor(int hidrogenianId) {
            _logger.LogInformation("AddressController.GetAddressListFor - hidrogenianId=" + hidrogenianId);
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AddressController),
                Action = nameof(GetAddressListFor),
                Data = hidrogenianId.ToString(),
                Briefing = "Get the list of all addresses for CAB.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var addressList = await ReadFromRedisCacheAsync<List<IGenericAddressVM>>("Profile_AddressList");
            if (addressList != null) return new JsonResult(new { Result = RESULTS.SUCCESS, Message = addressList });

            addressList = await _addressService.RetrieveAddressesForHidrogenian(hidrogenianId);
            if (addressList == null)
                return new JsonResult(new {Result = RESULTS.FAILED, Message = "An error occurred while retrieving data for location list. Please reload page to try again."});

            await InsertRedisCacheEntryAsync("Profile_AddressList", addressList);
            return new JsonResult(new { Result = RESULTS.SUCCESS, Message = addressList });
        }

        [HttpPost("add-address")]
        [HidroActionFilter(ROLES.CUSTOMER)]
        [HidroAuthorize(PERMISSIONS.CREATE)]
        public async Task<JsonResult> AddNewAddressFor(AddressBinderVM binder) {
            _logger.LogInformation("AddressController.GetAddressListFor - hidrogenianId=" + binder.HidrogenianId);
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AddressController),
                Action = nameof(AddNewAddressFor),
                Data = JsonConvert.SerializeObject(binder),
                Briefing = "Add a new address to database for the user.",
                Severity = LOGGING.INFORMATION.GetValue()
            });
            
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
            if (rawAddress == null)
                return new JsonResult(new {Result = RESULTS.FAILED, Message = "An error occurred while saving your address. Please try again."});
            
            await RemoveRedisCacheEntryAsync("Profile_AddressList");
            return new JsonResult(new { Result = RESULTS.SUCCESS, Message = rawAddress });
        }

        [HttpDelete("remove-address/{addressId}")]
        [HidroActionFilter(ROLES.CUSTOMER)]
        [HidroAuthorize(PERMISSIONS.DELETE_OWN)]
        public async Task<JsonResult> RemoveHidrogenianAddress(int addressId) {
            _logger.LogInformation("AddressController.RemoveHidrogenianAddress - addressId=" + addressId);
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AddressController),
                Action = nameof(RemoveHidrogenianAddress),
                Data = addressId.ToString(),
                Briefing = "Remove an address by ID from database for user.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var result = await _addressService.RemoveHidroAddress(addressId);
            if (!result.HasValue) return new JsonResult(new { Result = RESULTS.FAILED, Message = "No address found with the given data. Please try again." });
            if (!result.Value.Key) return new JsonResult(new { Result = RESULTS.FAILED, Message = "The address is currently set as either Primary or Delivery. Deletion cancelled." });
            if (!result.Value.Value) return new JsonResult(new {Result = RESULTS.FAILED, Message = "Error occurred while attempting to delete the address. Please try again."});
            
            await RemoveRedisCacheEntryAsync("Profile_AddressList");
            return new JsonResult(new { Result = RESULTS.SUCCESS });
        }

        [HttpPost("update-address")]
        [HidroActionFilter(ROLES.CUSTOMER)]
        [HidroAuthorize(PERMISSIONS.EDIT_OWN)]
        public async Task<JsonResult> UpdateHidrogenianAddress(AddressBinderVM binder) {
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AddressController),
                Action = nameof(UpdateHidrogenianAddress),
                Data = JsonConvert.SerializeObject(binder),
                Briefing = "Update an address in database for the user.",
                Severity = LOGGING.INFORMATION.GetValue()
            });
            
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

            var (updatedSuccess, newAddress) = await _addressService.UpdateHidroAddress(address);

            if (!updatedSuccess.HasValue)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "No address found with the given data. Please try again." });

            if (!updatedSuccess.Value)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "Error occurred while attempting to update the address details. Please try again." });

            if (newAddress == null)
                return new JsonResult(new {Result = RESULTS.FAILED, Message = "Error occurred while saving your new address details. Please try again."});
            
            await RemoveRedisCacheEntryAsync("Profile_AddressList");
            return new JsonResult(new { Result = RESULTS.SUCCESS, Message = newAddress });
        }
        
        [HttpPost("set-address-field")]
        [HidroActionFilter(ROLES.CUSTOMER)]
        [HidroAuthorize(PERMISSIONS.EDIT_OWN)]
        public async Task<JsonResult> SetAddressAsPrimaryOrDeliveryFor(AddressSetterVM data) {
            _logger.LogInformation("AddressController.SetAddressAsPrimaryFor - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AddressController),
                Action = nameof(SetAddressAsPrimaryOrDeliveryFor),
                Data = JsonConvert.SerializeObject(data),
                Briefing = "Set an address <ForDelivery> or <AsPrimary>.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var result = await _addressService.SetFieldDataForAddress(data);
            if (!result.HasValue) return new JsonResult(new {Result = RESULTS.FAILED, Message = "Unable to find the address with given data. Please check again."});
            if (!result.Value) return new JsonResult(new { Result = RESULTS.FAILED, Message = "An error occurred while attempting to update the address. Please try again." });
            
            await RemoveRedisCacheEntryAsync("Profile_AddressList");
            return new JsonResult(new {Result = RESULTS.SUCCESS});
        }

        private List<int> VerifyAddress(IGenericAddressVM address) {
            _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AddressController),
                Action = "private" + nameof(VerifyAddress),
                Data = JsonConvert.SerializeObject(address),
                Briefing = "Internally verify the submitted address data for any errors.",
                Severity = LOGGING.INFORMATION.GetValue()
            });
            
            var errors = new List<int>();
            var location = address.IsStandard ? address.sAddress : (GenericLocationVM)address.lAddress;

            errors.AddRange(location.VerifyBuildingName());
            errors.AddRange(location.VerifyPoBox());
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
