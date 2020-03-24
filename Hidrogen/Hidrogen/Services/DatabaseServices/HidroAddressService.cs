using Hidrogen.DbContexts;
using Hidrogen.Models;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels.Address;
using Hidrogen.ViewModels.Address.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hidrogen.Services.DatabaseServices {

    public class HidroAddressService : IHidroAddressService {

        private readonly ILogger<HidroAddressService> _logger;
        private HidrogenDbContext _dbContext;

        public HidroAddressService(
            ILogger<HidroAddressService> logger,
            HidrogenDbContext dbContext
        ) {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IGenericAddressVM> InsertRawAddressFor(int hidrogenianId, IGenericAddressVM address) {
            _logger.LogInformation("HidroAddressService.InsertRawAddressFor - hidrogenianId=" + hidrogenianId);

            var rawLocation = await InsertRawLocation(address);
            if (rawLocation == null) return null;

            var hidrogenianAddress = new HidroAddress {
                HidrogenianId = hidrogenianId,
                LocationId = rawLocation.Id,
                IsRefined = false,
                IsPrimaryAddress = address.IsPrimary,
                IsDeliveryAddress = address.ForDelivery
            };

            _dbContext.HidroAddress.Add(hidrogenianAddress);
            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("HidroAddressService.InsertRawAddressFor - Error at inserting hidroAddress: " + e.ToString());
                return null;
            }

            address.Id = hidrogenianAddress.Id;
            return address;
        }

        public async Task<bool?> RemoveHidroAddress(int addressId) {
            _logger.LogInformation("HidroAddressService.RemoveHidroAddress - addressId=" + addressId);

            var dbAddress = await _dbContext.HidroAddress.FindAsync(addressId);
            if (dbAddress == null) return null;

            _dbContext.HidroAddress.Remove(dbAddress);
            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("HidroAddressService.RemoveHidroAddress - Error: " + e.ToString());
                return false;
            }

            return true;
        }

        public async Task<List<IGenericAddressVM>> RetrieveAddressesForHidrogenian(int hidrogenianId) {
            _logger.LogInformation("HidroAddressService.RetrieveAddressesForHidrogenian - Service starts.");

            var vmAddresses = new List<IGenericAddressVM>();
            try {
                var hidroAddresses = await _dbContext.HidroAddress
                                                     .Where(ha => ha.HidrogenianId == hidrogenianId)
                                                     .Select(ha => ha).ToListAsync();

                foreach (var address in hidroAddresses) {
                    RawLocation rawLocation; FineLocation fineLocation;

                    if (address.IsRefined) {
                        fineLocation = await _dbContext.FineLocation.FindAsync(address.LocationId.Value);

                        if (fineLocation.IsStandard) {
                            var standardFineAddress = (StandardAddressVM)fineLocation;
                            standardFineAddress.SetAddressValues(address);

                            vmAddresses.Add(standardFineAddress);
                        }
                        else {
                            var localFineAddress = (LocalAddressVM)fineLocation;
                            localFineAddress.SetAddressValues(address);

                            vmAddresses.Add(localFineAddress);
                        }
                    }
                    else {
                        rawLocation = await _dbContext.RawLocation.FindAsync(address.LocationId.Value);

                        if (rawLocation.IsStandard) {
                            var standardRawLocation = (StandardAddressVM)rawLocation;
                            standardRawLocation.SetAddressValues(address);

                            vmAddresses.Add(standardRawLocation);
                        }
                        else {
                            var localRawLocation = (LocalAddressVM)rawLocation;
                            localRawLocation.SetAddressValues(address);

                            vmAddresses.Add(localRawLocation);
                        }
                    }

                }
            } catch (Exception e) {
                _logger.LogError("HidroAddressService.RetrieveAddressesForHidrogenian - Error: " + e.ToString());
                return null;
            }

            return vmAddresses;
        }

        public async Task<KeyValuePair<bool?, IGenericAddressVM>> UpdateHidroAddress(IGenericAddressVM address) {
            _logger.LogInformation("HidroAddressService.UpdateHidroAddress - Service starts.");

            var dbAddress = await _dbContext.HidroAddress.FindAsync(address.Id);
            if (dbAddress == null) return new KeyValuePair<bool?, IGenericAddressVM>(null, null);

            var rawLocation = await InsertRawLocation(address);
            if (rawLocation == null) return new KeyValuePair<bool?, IGenericAddressVM>(false, null);

            dbAddress.LocationId = rawLocation.Id;
            dbAddress.IsDeliveryAddress = address.ForDelivery;
            dbAddress.IsPrimaryAddress = address.IsPrimary;
            dbAddress.IsRefined = false;

            _dbContext.HidroAddress.Update(dbAddress);
            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("HidroAddressService.UpdateHidroAddress - Error: " + e.ToString());
                return new KeyValuePair<bool?, IGenericAddressVM>(true, null);
            }

            if (address.IsStandard) address._sAddress.Id = rawLocation.Id;
            else address._lAddress.Id = rawLocation.Id;

            address.IsRefined = false;
            return new KeyValuePair<bool?, IGenericAddressVM>(true, address);
        }

        private async Task<RawLocation> InsertRawLocation(IGenericAddressVM address) {
            _logger.LogInformation("HidroAddressService.InsertRawLocation - Service runs internally.");

            RawLocation rawLocation;
            if (address.IsStandard) {
                rawLocation = address._sAddress;
                rawLocation.IsStandard = address.IsStandard;
            }
            else {
                rawLocation = address._lAddress;
                rawLocation.IsStandard = address.IsStandard;
            }

            _dbContext.RawLocation.Add(rawLocation);
            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("HidroAddressService.InsertRawAddressFor - Error at inserting rawLocation: " + e.ToString());
                return null;
            }

            return rawLocation;
        }
    }
}