using Hidrogen.ViewModels;
using Hidrogen.ViewModels.Authentication;
using System.Threading.Tasks;

namespace Hidrogen.Services.Interfaces {

    public interface IHidrogenianService {

        /// <summary>
        /// Returns null if database transaction failed.
        /// </summary>
        Task<HidrogenianVM> InsertNewHidrogenian(RegistrationVM registration);

        /// <summary>
        /// Retrurns false if database transaction failed.
        /// </summary>
        Task<bool> SetAccountConfirmationToken(HidrogenianVM hidrogenian);

        /// <summary>
        /// Retrurns false if database transaction failed.
        /// </summary>
        Task<bool> RemoveNewlyInsertedHidrogenian(int hidrogenianId);

        /// <summary>
        /// Returns null if no account satisfied conditions.
        /// </summary>
        Task<HidrogenianVM> GetHidrogenianByEmail(string email);

        /// <summary>
        /// Returns null if no account satisfied conditions.
        /// </summary>
        Task<HidrogenianVM> GetUnactivatedHidrogenianByEmail(string email);
    }
}
