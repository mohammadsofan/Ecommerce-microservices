using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IdentityService.Application.Dtos.Auth;
using IdentityService.Domain.Entities;

namespace IdentityService.Application.Mappings
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            CreateMap<User, RegisterResDto>();

           CreateMap<RegisterReqDto, User>()
            .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src =>
                new List<Address> { new() {
                    Street = src.Address.Street,
                    City = src.Address.City,
                    State = src.Address.State,
                    ZipCode = src.Address.ZipCode,
                    Country = src.Address.Country
                }}));


        }
    }
}