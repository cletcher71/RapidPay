using System.ComponentModel.DataAnnotations;

namespace RapidPay.Models
{
    public class CreditCardModel
    {
        public Guid Id { get; set; }

        public string PrimaryAccountNumber { get; set; }

        public string PrimaryAccountNumberMasked { get; set; }

        public int CVV { get; set; }

        public float ExpirationMonth { get; set; }

        public float ExpirationYear { get; set; }

        public string NameOnCard { get; set; }

        public string PostalCode { get; set; }

        public string Type { get; set; }

        public decimal CreditLimit { get; set; }

        public decimal Balance { get; set; }

    }
}

