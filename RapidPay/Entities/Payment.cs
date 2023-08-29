using System.ComponentModel.DataAnnotations;

namespace RapidPay.Entities
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        public Guid PaymentUuid { get; set; }

        public CreditCard CreditCard { get; set; }

        public decimal Amount { get; set; }

        public decimal FeeAmount { get; set; }

        public DateTime DatePaid { get; set; }
    }
}
