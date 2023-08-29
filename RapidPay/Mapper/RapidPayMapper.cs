using AutoMapper;
using RapidPay.Models;
using RapidPay.Entities;

namespace RapidPay.Mapping
{
    public class RapidPayMappingProfile : Profile
    {
        public RapidPayMappingProfile()
        {
            CreateMap<CreditCard, CreditCardModel>()
                    .ForMember(d => d.Id, opt => opt.MapFrom(s => s.CreditCardUuid))
                    .ForMember(d => d.Id, opt => opt.DoNotUseDestinationValue());

            CreateMap<CreditCardModel, CreditCard>()
                    .ForMember(d => d.CreditCardUuid, opt => opt.MapFrom(s => s.Id))
                    .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Payment, PaymentModel>()
                    .ForMember(d => d.Id, opt => opt.MapFrom(s => s.PaymentUuid))
                    .ForMember(d => d.Id, opt => opt.DoNotUseDestinationValue());

            CreateMap<PaymentModel, Payment>()
                    .ForMember(d => d.PaymentUuid, opt => opt.MapFrom(s => s.Id))
                    .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<PaymentFee, PaymentFeeModel>()
                    .ForMember(d => d.Id, opt => opt.MapFrom(s => s.PaymentFeeUuid))
                    .ForMember(d => d.Id, opt => opt.DoNotUseDestinationValue());

            CreateMap<PaymentFeeModel, PaymentFee>()
                    .ForMember(d => d.PaymentFeeUuid, opt => opt.MapFrom(s => s.Id))
                    .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}