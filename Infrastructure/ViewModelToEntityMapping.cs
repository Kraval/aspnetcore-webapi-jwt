using AutoMapper;
using AspNetCoreWebApiJwt.ViewModels;
using AspNetCoreWebApiJwt.Models;

namespace AspNetCoreWebApiJwt.Infrastructure
{
    public class ViewModelToEntityMappingProfile : Profile
    {
        public ViewModelToEntityMappingProfile()
        {
            CreateMap<RegisterViewModel, ApplicationUser>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.Email));
        }
    }
}