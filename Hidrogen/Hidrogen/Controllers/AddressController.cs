using Hidrogen.Attributes;
using Hidrogen.Services;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels.Address.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Hidrogen.Controllers {

    [ApiController]
    [Route("profile")]
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
            throw new NotImplementedException();
        }

        [HttpPost("add-address/{hidrogenianId}")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("1,0,0,0,0,0,0,0")]
        public async Task<JsonResult> AddNewAddressFor(
            [FromQuery] int hidrogenianId,
            [FromBody] IGenericAddressVM address
        ) {
            throw new NotImplementedException();
        }

        [HttpDelete("remove-address/{addressId}")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,0,0,0,1,0,0,0")]
        public async Task<JsonResult> RemoveHidrogenianAddress(int addressId) {
            throw new NotImplementedException();
        }

        [HttpPut("update-address")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,0,1,0,0,0,0,0")]
        public async Task<JsonResult> UpdateHidrogenianAddress(IGenericAddressVM address) {
            throw new NotImplementedException();
        }

        [HttpPost("get-similar-address")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,1,0,0,0,0,0,0")]
        public async Task<JsonResult> GetAddressesSimilarTo(IGenericAddressVM address) {
            throw new NotImplementedException();
        }
    }
}
