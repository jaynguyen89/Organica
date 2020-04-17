using System.Linq;
using System.Threading.Tasks;
using Hidrogen.DbContexts;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hidrogen.Services.DatabaseServices {
    
    public class CountryService : ICountryService {
        
        private readonly ILogger<CountryService> _logger;
        private readonly HidrogenDbContext _dbContext;

        public CountryService(
            ILogger<CountryService> logger,
            HidrogenDbContext dbContext
        ) {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<CountryVM[]> GetCompactCountries() {
            _logger.LogInformation("CountryService.GetCompactCountries - Service starts.");

            var countries = await _dbContext.Country
                .Select(c => new CountryVM {
                    Id = c.Id,
                    Name = c.CountryName,
                    Code = c.CountryCode
                }).OrderBy(c => c.Name).ToArrayAsync();

            return countries;
        }

        public async Task<CountryVM[]> GetAllCountries() {
            _logger.LogInformation("CountryService.GetAllCountries - Service starts.");

            var countries = await _dbContext.Country
                .Select(c => new CountryVM {
                    Id = c.Id,
                    Name = c.CountryName,
                    Code = c.CountryCode,
                    Continent = c.Continent,
                    Currency = new CurrencyVM {
                        Name = c.CurrencyName,
                        Code = c.CurrencyCode
                    }
                }).OrderBy(c => c.Name).ToArrayAsync();

            return countries;
        }

        public async Task<CurrencyVM> GetCurrencyByCountryId(int id) {
            _logger.LogInformation("CountryService.GetCurrencyByCountryId - Service starts.");

            var country = await _dbContext.Country.FindAsync(id);
            if (country == null) return null;
            
            return new CurrencyVM {
                Name = country.CurrencyName,
                Code = country.CurrencyCode
            };
        }

        public async Task<CurrencyVM> GetCurrencyByCountryNameOrCode(string needle) {
            _logger.LogInformation("CountryService.GetCurrencyByCountryNameOrCode - needle=" + needle);

            var country = await _dbContext.Country
                .FirstOrDefaultAsync(c => c.CountryName.ToLower() == needle.ToLower()) ?? await _dbContext.Country
                .FirstOrDefaultAsync(c => c.CountryCode.ToUpper() == needle.ToUpper());

            if (country == null) return null;
            
            return new CurrencyVM {
                Name = country.CurrencyName,
                Code = country.CurrencyCode
            };
        }
    }
}