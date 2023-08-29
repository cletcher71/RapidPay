namespace RapidPay.Models
{
    public class PaymentResultModel
    {
        public Guid id { get; set; }

        public string IssuerResponseCode { get; set; }

        public string IssuerResponseDescription { get; set;  }

        public string GatewayResponseCode { get; set; }

        public string GatewayResponseDescription { get; set; }

        public PaymentModel Payment { get; set;  }
    }
}
