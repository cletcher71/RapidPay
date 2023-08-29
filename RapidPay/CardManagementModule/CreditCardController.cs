using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RapidPay.Entities;
using RapidPay.Models;
using RapidPay.Services;
using System.Diagnostics.Eventing.Reader;
using System.Reflection.PortableExecutable;

namespace RapidPay.CardManagementModule
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CreditCardController : ControllerBase
    {

        private readonly ILogger<CreditCardController> _logger;

        private readonly ApplicationDbContext _dbContext;

        private IMapper _mapper;

        private readonly IConfiguration _configuration;

        private CreditCardService _creditCardService;

        public CreditCardController(ILogger<CreditCardController> logger, ApplicationDbContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpPost("~/CreateCard")]
        public ActionResult<CreditCardModel> CreateCard([FromBody] String creditCardModel)
        {
            if (creditCardModel != null)
            {
                var _creditCardModel = JsonConvert.DeserializeObject<CreditCardModel>(creditCardModel);

                if (_creditCardModel is not CreditCardModel)
                {
                    return new BadRequestResult();

                }
                else
                {
                    using (var context = _dbContext)
                    {

                        var card = _dbContext.CreditCards.Where(c => c.CreditCardUuid == _creditCardModel.Id).FirstOrDefault();

                        if (card is not CreditCard)
                        {
                            _dbContext.Add(_mapper.Map<CreditCard>(_creditCardModel));
                            _dbContext.SaveChanges();
                        }
                    }
                }
                return Ok(creditCardModel);
            }
            else
            {
                return new BadRequestResult();
            }
        }

        [HttpPost("~/Pay")]
        public ActionResult<PaymentModel> Pay([FromBody] String paymentModel)
        {
            PaymentResultModel paymentResultModel = new PaymentResultModel();

            paymentResultModel.id = Guid.NewGuid();

            if (paymentModel != null)
            {
                var _paymentModel = JsonConvert.DeserializeObject<PaymentModel>(paymentModel);

                if (_paymentModel is not PaymentModel)
                {
                    return new BadRequestResult();
                }
                else
                {
                    // This entire section should be in a service and a workflow class to process all of the return codes properly and cleanly with extensive logging.
                    // This shows some basic return code and card validation. Codes below I knew from working with Heartland ACH at my last job.

                    using (var context = _dbContext)
                    {

                        var payment = context.Payments.Where(p => p.PaymentUuid == _paymentModel.Id).FirstOrDefault();

                        if (payment is not Payment)
                        {
                            var TodayMonth = DateTime.Now.Month;
                            var TodayYear = DateTime.Now.Year;
                            var ExpirationMonth = (int)_paymentModel.CreditCard.ExpirationMonth;
                            var ExpirationYear = ((int)_paymentModel.CreditCard.ExpirationYear) + 2000;

                            if ((ExpirationYear < TodayYear) || (ExpirationYear == TodayYear && ExpirationMonth < TodayMonth))
                            {
                                // Decline payment because card is expired

                                paymentResultModel.Payment = _paymentModel;
                                paymentResultModel.IssuerResponseCode = "54";
                                paymentResultModel.IssuerResponseDescription = "EXPIRED CARD—Card is expired.";
                                paymentResultModel.GatewayResponseCode = "0";
                                paymentResultModel.GatewayResponseDescription = "Success";
                                return Ok(paymentResultModel);
                            }

                            var creditLimit = _paymentModel.CreditCard.CreditLimit;
                            var balance = _paymentModel.CreditCard.Balance;
                            var paymentAmount = _paymentModel.Amount;

                            var availableCredit = creditLimit - balance;

                            if (paymentAmount <= availableCredit)
                            {

                                payment = _mapper.Map<Payment>(_paymentModel);

                                payment.CreditCard.Balance = balance + paymentAmount;

                                context.Add(payment);
                                context.SaveChanges();

                                var paymentResult = _mapper.Map<PaymentModel>(_paymentModel);

                                // Mask/Anonymize PrimaryAccountNumber and CVV in response payload

                                paymentResult.CreditCard.PrimaryAccountNumber = paymentResult.CreditCard.PrimaryAccountNumberMasked;
                                paymentResult.CreditCard.CVV = 0;

                                paymentResultModel.Payment = paymentResult;
                                paymentResultModel.IssuerResponseCode = "00";
                                paymentResultModel.IssuerResponseDescription = "APPROVAL";
                                paymentResultModel.GatewayResponseCode = "0";
                                paymentResultModel.GatewayResponseDescription = "Success";
                            }
                            else
                            {
                                // Decline payment because balance is too high

                                paymentResultModel.Payment = _paymentModel;
                                paymentResultModel.IssuerResponseCode = "51";
                                paymentResultModel.IssuerResponseDescription = "DECLINE—Insufficient funds.";
                                paymentResultModel.GatewayResponseCode = "0";
                                paymentResultModel.GatewayResponseDescription = "Success";
                                return Ok(paymentResultModel);
                            }
                        }
                    }
                }
            }

            return Ok(paymentResultModel);
        }

        [HttpGet("~/GetCardBalance/{id}")]
        public ActionResult<CreditCardModel> GetCardBalance([FromRoute] Guid id)
        {

            CreditCardModel creditCardModel = new CreditCardModel();

            using (var context = _dbContext)
            {

                var card = context.CreditCards.Where(c => c.CreditCardUuid == id).FirstOrDefault();

                if (card is not CreditCard)
                {
                    return NotFound();
                }
                else
                {
                    creditCardModel = _mapper.Map<CreditCardModel>(card);
                }
            }

            return Ok(creditCardModel.Balance);
        }
    }
}
