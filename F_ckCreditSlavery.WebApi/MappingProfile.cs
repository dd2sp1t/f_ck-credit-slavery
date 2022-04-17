using AutoMapper;
using F_ckCreditSlavery.Entities.DataTransferObjects;
using F_ckCreditSlavery.Entities.Models;

namespace F_ckCreditSlavery.WebApi;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreditAccount, CreditAccountGetDto>();
        CreateMap<CreditAccountPostDto, CreditAccount>();
        CreateMap<CreditAccountUpdateDto, CreditAccount>();

        CreateMap<CreditAccountChange, CreditAccountChangeGetDto>();
        CreateMap<CreditAccountChangePostDto, CreditAccountChange>();
        CreateMap<CreditAccountChangeUpdateDto, CreditAccountChange>();

        CreateMap<UserRegisterDto, User>();
    }
}