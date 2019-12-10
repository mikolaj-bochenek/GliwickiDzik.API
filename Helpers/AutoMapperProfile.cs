using AutoMapper;
using GliwickiDzik.API.DTOs;
using GliwickiDzik.DTOs;
using GliwickiDzik.Models;

namespace GliwickiDzik.API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Method<what map, map to what>();
            CreateMap<UserForRegisterDTO, UserModel>();
            CreateMap<UserForEditDTO, UserModel>();
        }
    }
}