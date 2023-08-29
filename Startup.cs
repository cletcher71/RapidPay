using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RapidPay.Entities;
using RapidPay.Mapping;
using RapidPay.Services;

namespace RapidPay
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        }

        public void ConfigureServices(IServiceCollection services)
        {

            var connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer
                (connectionString)
            );

            // Auto Mapper Configuration
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new RapidPayMappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            //services.AddAutoMapper(typeof(Startup));

            

            services.AddScoped<IUniversalFeesExchangeService, UniversalFeesExchangeService>();
            services.AddSingleton<ICreditCardService, CreditCardService>();
            services.AddSingleton<IPaymentFeeService, PaymentFeeService>();
            //services.AddScheduler((sender, args) =>
            //{
            //    Console.Write(args.Exception.Message);
            //    args.SetObserved();
            //});

            services.AddHttpClient("client", c =>
            {
                c.BaseAddress = new Uri(Configuration.GetValue<string>("https://localhost:1232"));
            });
        }
    }
}
