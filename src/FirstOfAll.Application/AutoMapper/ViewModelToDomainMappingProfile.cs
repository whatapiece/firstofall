using AutoMapper;
using FirstOfAll.Application.ViewModels;
using FirstOfAll.Domain.Models;

namespace FirstOfAll.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile() => CreateMap<CustomerViewModel, Customer>();
    }
}
