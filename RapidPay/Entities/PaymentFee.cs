
using System;
using System.ComponentModel.DataAnnotations;

namespace RapidPay.Entities
{
    public class PaymentFee
    {
        [Key]
        public int Id { get; set; }

        public Guid PaymentFeeUuid { get; set; }

        public decimal Amount { get; set; }

        public DateTime DateCreated { get; set; }

    }
}