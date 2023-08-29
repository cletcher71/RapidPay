using RapidPay.Models;

namespace RapidPay.Services
{
    public interface IUniversalFeesExchangeService
    {
        PaymentFeeModel GetCurrentFee();

    }
}
