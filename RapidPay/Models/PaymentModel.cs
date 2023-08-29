using RapidPay.Entities;
using System.ComponentModel.DataAnnotations;

namespace RapidPay.Models
{
    public class PaymentModel
    {
        public Guid Id { get; set; }

        public CreditCard CreditCard { get; set; }

        public decimal Amount { get; set; }

        public decimal FeeAmount { get; set; }

        public DateTime DatePaid { get; set; }
    }

    //{ Other properties that would be needed perhaps
    //    "paymentMethodToken": "bb3vph6",
    //    "paymentMethodTypes": "ACH",
    //    "transactionAmount": "1090.9",
    //    "transactionType": "Charge",
    //    "transactionID": "3532464245",
    //    "merchantAccountID": "643765867",
    //    "insuredName": "First Last",
    //    "firstName": "First",
    //    "lastName": "Last",
    //    // "sourceUserKey": "example@gmail.com"
    //    "paymentMethodTokenType": "Durable",
    //    "paymentType": "Recurring",
    //    "sourceTransactionId": "OrderACH300"
    //}
}
