using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IdentityService.Application.Dtos.Address;
using IdentityService.Domain.Entities;

namespace IdentityService.Application.Mappings
{
    public class AddressMappingProfile : Profile
    {
        public AddressMappingProfile()
        {
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<Address, CreateAddressDto>().ReverseMap();
            CreateMap<UpdateAddressDto, Address>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}