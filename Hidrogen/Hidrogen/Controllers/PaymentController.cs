using Hidrogen.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hidrogen.Controllers {

    [ApiController]
    [Route("payment")]
    public class PaymentController {

        public readonly ILogger<PaymentController> _logger;
        public readonly IPaymentService _paymentService;

        public PaymentController(
            ILogger<PaymentController> logger,
            IPaymentService paymentService
        ) {
            _logger = logger;
            _paymentService = paymentService;
        }
    }
}
