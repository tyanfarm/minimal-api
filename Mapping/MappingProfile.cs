using AutoMapper;
using SimpleMinimalAPI.DTOs;
using SimpleMinimalAPI.Models;

namespace SimpleMinimalAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        { 
            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.PasswordHash,
                        opt => opt.MapFrom( new PasswordHashResolver()) );
        }

    }
}
