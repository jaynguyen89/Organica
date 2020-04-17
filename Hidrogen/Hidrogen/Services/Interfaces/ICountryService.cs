using System.Threading.Tasks;
using Hidrogen.ViewModels;

namespace Hidrogen.Services.Interfaces {
    public interface ICountryService {

        Task<CountryVM[]> GetCompactCountries();

        Task<CountryVM[]> GetAllCountries();

        Task<CurrencyVM> GetCurrencyByCountryId(int id);

        Task<CurrencyVM> GetCurrencyByCountryNameOrCode(string needle);
    }
}