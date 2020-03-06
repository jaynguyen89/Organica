using Hidrogen.ViewModels;
using System.Threading.Tasks;

namespace Hidrogen.Services.Interfaces {

    public interface IHidrogenianService {

        Task<HidrogenianVM> InsertNewHidrogenian(RegistrationVM registration);

        Task<bool> SetAccountConfirmationToken(HidrogenianVM hidrogenian);

        Task<bool> RemoveNewlyInsertedHidrogenian(int hidrogenianId);
    }
}
