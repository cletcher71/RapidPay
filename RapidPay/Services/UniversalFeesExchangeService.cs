using AutoMapper;
using RapidPay.CardManagementModule;
using RapidPay.Entities;
using RapidPay.Models;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Timers;

namespace RapidPay.Services
{
    public class UniversalFeesExchangeService : IUniversalFeesExchangeService
    {
        private ILogger<UniversalFeesExchangeService> _logger;

        //private readonly HttpClientFactory clientFactory;

        private ApplicationDbContext _applicationDbContext;

        private IMapper _mapper;

        private System.Timers.Timer timer1 = new System.Timers.Timer();

        public UniversalFeesExchangeService(IMapper mapper, ILogger<UniversalFeesExchangeService> logger, ApplicationDbContext applicationDBContext) //ApplicationDbContext applicationDBContext,, HttpClientFactory clientFactory
        {
            _applicationDbContext = applicationDBContext;
            _mapper = mapper;
            _logger = logger;
            //_clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));

            timer1.Elapsed += new ElapsedEventHandler(Timer_Tick);
            timer1.Enabled = true;
            timer1.Interval = MillisecondsToHour();
            _logger.LogInformation("Timer sterted");
        }

        public PaymentFeeModel GetCurrentFee()
        {
            var paymentFee = _applicationDbContext.Fees.Where(c => c.DateCreated > DateTime.Today.AddDays(-1))
                                .OrderByDescending(t => t.DateCreated)
                                .FirstOrDefault();

            PaymentFeeModel paymentFeeModel = _mapper.Map<PaymentFeeModel>(paymentFee);

            return paymentFeeModel;
        }

        private int MillisecondsToHour()
        {
            int interval;

            int minutesRemaining = 59 - DateTime.Now.Minute;
            int secondsRemaining = 59 - DateTime.Now.Second;
            interval = ((minutesRemaining * 60) + secondsRemaining) * 1000;

            if (interval == 0)
            {
                interval = 60 * 60 * 1000;
            }

            interval = 60;

            return interval;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _logger.LogInformation("Timer ticked");

            timer1.Interval = MillisecondsToHour();
            CreateNextFee();
        }

        private void CreateNextFee()
        {
            try
            {
                var rand = new Random();
                var newFeeInt = new decimal(rand.NextDouble()) * 2;

                var lastFee = _applicationDbContext.Fees.Where(c => c.DateCreated > DateTime.Today.AddDays(-1))
                                  .OrderByDescending(t => t.DateCreated)
                                  .FirstOrDefault();


                var fee = newFeeInt * lastFee.Amount;

                var dateCreated = lastFee.DateCreated.AddHours(1);

                PaymentFee paymentFee = new PaymentFee
                {
                    PaymentFeeUuid = Guid.NewGuid(),
                    Amount = fee,
                    DateCreated = dateCreated
                };

                _applicationDbContext.Fees.Add(paymentFee);
                _applicationDbContext.SaveChanges();

                _logger.LogInformation($"Created next fee: {fee}");
            }
            catch (Exception)
            {
                _logger.LogError($"CreateNextFee");
            }
        }

        private Tuple<decimal,decimal> GetLatestFee(decimal fee)
        {
            Tuple<decimal, decimal> fees = new Tuple<decimal, decimal>(1, 2);
            return fees;
        }

        private Tuple<decimal, decimal> CreateFirstFees()
        {
            Tuple<decimal, decimal> fees = new Tuple<decimal, decimal>(1,2);
            return fees;
        }

        //private PaymentFeeModel GetLatestFee()
        //{
        //    // Create client
        //    HttpClient client = _clientFactory.CreateClient("THE_Client");

        //    // send request
        //    var builder = new UriBuilder(client.BaseAddress)
        //    {
        //        Path = "api/v1/ENDPOINT"
        //    };

        //    var request = new HttpRequestMessage
        //    {
        //        RequestUri = builder.Uri,
        //        Method = HttpMethod.Post,
        //        Content = "blah"
        //    };

        //    var response = await client.SendAsync(request);

        //    response.EnsureSuccessStatusCode();

        //    return new PaymentFeeModel();
        //}
    }
}
