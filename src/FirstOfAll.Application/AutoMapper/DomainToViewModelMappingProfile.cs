using AutoMapper;
using FirstOfAll.Application.ViewModels;
using FirstOfAll.Domain.Models;

namespace FirstOfAll.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile() => CreateMap<Customer, CustomerViewModel>();

    }
}
