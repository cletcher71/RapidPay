namespace RapidPay.Models
{
    public class PaymentFeeModel
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
