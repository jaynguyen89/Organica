using Hidrogen.DbContexts;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels.Address;
using Hidrogen.ViewModels.Address.Generic;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

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
    }
}