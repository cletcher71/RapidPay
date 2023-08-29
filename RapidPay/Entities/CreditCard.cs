
using System;
using System.ComponentModel.DataAnnotations;

namespace RapidPay.Entities
{
    public class CreditCard
    {
        //class CreditCard
        //{
        //    public int Id = new 
        //}

        [Key]
        public int Id { get; set; }

        public Guid CreditCardUuid { get; set; }

        public string PrimaryAccountNumber { get;  set; }

        public string PrimaryAccountNumberMasked { get; set; }

        public int CVV { get; set; }

        public float ExpirationMonth { get; set; }

        public float ExpirationYear { get; set; }

        public string NameOnCard { get; set; }

        public string PostalCode { get; set; }

        public string Type { get; set;  }

        public decimal CreditLimit { get; set; }

        public decimal Balance { get; set; }

    }
}