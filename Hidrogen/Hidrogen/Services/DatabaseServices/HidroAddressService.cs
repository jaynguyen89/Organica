using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hidrogen.DbContexts;
using Hidrogen.Models;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels.Address;
using Hidrogen.ViewModels.Address.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hidrogen.Services.DatabaseServices {

    public class HidroAddressService : IHidroAddressService {

        private readonly ILogger<HidroAddressService> _logger;
        private readonly HidrogenDbContext _dbContext;

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
                Title = address.Title,
                IsRefined = false,
                IsPrimaryAddress = false,
                IsDeliveryAddress = false
            };

            _dbContext.HidroAddress.Add(hidrogenianAddress);
            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("HidroAddressService.InsertRawAddressFor - Error at inserting hidroAddress: " + e);
                return null;
            }

            address.Id = hidrogenianAddress.Id;
            return address;
        }

        public async Task<KeyValuePair<bool, bool>?> RemoveHidroAddress(int addressId) {
            _logger.LogInformation("HidroAddressService.RemoveHidroAddress - addressId=" + addressId);

            var dbAddress = await _dbContext.HidroAddress.FindAsync(addressId);
            if (dbAddress == null) return null;
            
            if (dbAddress.IsPrimaryAddress || dbAddress.IsDeliveryAddress)
                return new KeyValuePair<bool, bool>(false, false);

            _dbContext.HidroAddress.Remove(dbAddress);
            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("HidroAddressService.RemoveHidroAddress - Error: " + e);
                return new KeyValuePair<bool, bool>(true, false);
            }

            return new KeyValuePair<bool, bool>(true, true);
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
                        fineLocation.Country = await _dbContext.Country.FindAsync(fineLocation.CountryId);

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
                        rawLocation.Country = await _dbContext.Country.FindAsync(rawLocation.CountryId);

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
                _logger.LogError("HidroAddressService.RetrieveAddressesForHidrogenian - Error: " + e);
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
            dbAddress.Title = address.Title;
            dbAddress.IsDeliveryAddress = false;
            dbAddress.IsPrimaryAddress = false;
            dbAddress.IsRefined = false;
            dbAddress.LastUpdated = DateTime.UtcNow;

            _dbContext.HidroAddress.Update(dbAddress);
            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("HidroAddressService.UpdateHidroAddress - Error: " + e);
                return new KeyValuePair<bool?, IGenericAddressVM>(true, null);
            }

            if (address.IsStandard) address.sAddress.Id = rawLocation.Id;
            else address.lAddress.Id = rawLocation.Id;

            address.IsRefined = false;
            return new KeyValuePair<bool?, IGenericAddressVM>(true, address);
        }

        public async Task<bool?> SetFieldDataForAddress(AddressSetterVM data) {
            _logger.LogInformation("HidroAddressService.SetFieldDataForAddress - AddressId=" + data.Id);

            var hidrogenianAddresses = await _dbContext.HidroAddress
                .Where(a => a.HidrogenianId == data.HidrogenianId).ToListAsync();

            var addressesToUpdate = new List<HidroAddress>();
            foreach (var address in hidrogenianAddresses) {
                if (address.Id == data.Id) {
                    address.IsDeliveryAddress = string.Equals(data.Field, nameof(address.IsDeliveryAddress), StringComparison.CurrentCultureIgnoreCase);
                    address.IsPrimaryAddress = string.Equals(data.Field, nameof(address.IsPrimaryAddress), StringComparison.CurrentCultureIgnoreCase);
                    
                    addressesToUpdate.Add(address);
                    continue;
                }

                switch (data.Field) {
                    case nameof(address.IsDeliveryAddress) when address.IsDeliveryAddress:
                        address.IsDeliveryAddress = false;
                        break;
                    case nameof(address.IsPrimaryAddress) when address.IsPrimaryAddress:
                        address.IsPrimaryAddress = false;
                        break;
                }

                addressesToUpdate.Add(address);
            }

            if (addressesToUpdate.Count != 2) return null;
            
            _dbContext.HidroAddress.UpdateRange(addressesToUpdate);
            try {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e) {
                _logger.LogError("HidroAddressService.SetFieldDataForAddress - Error: " + e);
                return false;
            }

            return true;
        }

        private async Task<RawLocation> InsertRawLocation(IGenericAddressVM address) {
            _logger.LogInformation("HidroAddressService.InsertRawLocation - Service runs internally.");

            RawLocation rawLocation;
            if (address.IsStandard) {
                rawLocation = address.sAddress;
                rawLocation.IsStandard = address.IsStandard;
            }
            else {
                rawLocation = address.lAddress;
                rawLocation.IsStandard = address.IsStandard;
            }

            _dbContext.RawLocation.Add(rawLocation);
            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("HidroAddressService.InsertRawAddressFor - Error at inserting rawLocation: " + e);
                return null;
            }

            return rawLocation;
        }
    }
}