using AutoMapper;
using RapidPay.CardManagementModule;
using RapidPay.Entities;

namespace RapidPay.Services
{
    public class CreditCardService : ICreditCardService
    {

        private readonly ILogger<CreditCardController> _logger;

        private ApplicationDbContext _dbContext;

        private IMapper _mapper;

        private readonly IConfiguration _configuration;

        private PaymentFeeService _paymentFeeService;

        public CreditCardService(ILogger<CreditCardController> logger, ApplicationDbContext dbContext, IMapper mapper, IConfiguration configuration, PaymentFeeService paymentFeeService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
            _paymentFeeService = paymentFeeService; 
        }
    }
}
