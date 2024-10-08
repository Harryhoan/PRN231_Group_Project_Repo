using Application.ViewModels;
using Application.ViewModels.UserDTO;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mappers
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            CreateMap<User, cRegisterDTO>().ReverseMap();
            CreateMap<User, cLoginUserDTO>().ReverseMap();
            CreateMap<Koi, cCreateKOIDTO>().ReverseMap();
            CreateMap<User, cUserDTO>().ReverseMap();
            CreateMap<Koi, cKOIDTO>().ReverseMap();
        }
    }
}
