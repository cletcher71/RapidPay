using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RapidPay.CardManagementModule;
using RapidPay.Entities;
using RapidPay.Models;
using RapidPay.Services;

namespace RapidPay.PaymentFeesModule
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PaymentFeeController : ControllerBase
    {

        private readonly ILogger<PaymentFeeController> _logger;

        private readonly ApplicationDbContext _dbContext;

        private IMapper _mapper;

        private readonly IConfiguration _configuration;

        private UniversalFeesExchangeService _iUniversalFeesExchangeService;

        public PaymentFeeController(ILogger<PaymentFeeController> logger, ApplicationDbContext dbContext, IMapper mapper, IConfiguration configuration, UniversalFeesExchangeService universalFeesExchangeService)
        {
            _logger = logger;
            _iUniversalFeesExchangeService = universalFeesExchangeService;
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet("~/GetPaymentFee/{amount}")]

        public ActionResult<PaymentFeeModel> GetPaymentFee([FromRoute] decimal amount)
        {

            var paymentFeeModel = _dbContext.Fees.Where(c => c.DateCreated > DateTime.Today.AddDays(-1))
                                  .OrderByDescending(t => t.DateCreated)
                                  .FirstOrDefault();

            return Ok(paymentFeeModel);
        }
    }
}

