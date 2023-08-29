using RapidPay.Services;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;

namespace RapidPay.Entities
{
    public class ApplicationDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        //public ApplicationDbContext(IConfiguration configuration)
        //{
        //    //Configuration = configuration;
        //}

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<CreditCard> CreditCards { get; set; }

        public DbSet<PaymentFee> Fees { get; set; }

        public DbSet<Payment> Payments { get; set; }


    }
}