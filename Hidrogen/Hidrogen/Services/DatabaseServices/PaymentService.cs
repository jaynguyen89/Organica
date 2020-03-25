using Hidrogen.DbContexts;
using Hidrogen.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Hidrogen.Services.DatabaseServices {

    public class PaymentService : IPaymentService {

        private readonly ILogger<PaymentService> _logger;
        private HidrogenDbContext _dbContext;

        public PaymentService(
            ILogger<PaymentService> logger,
            HidrogenDbContext dbContext
        ) {
            _logger = logger;
            _dbContext = dbContext;
        }


    }
}
