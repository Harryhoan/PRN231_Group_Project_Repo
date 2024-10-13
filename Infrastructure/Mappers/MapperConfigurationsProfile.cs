using Application.ViewModels;
using Application.ViewModels.CategoryDTO;
using Application.ViewModels.KoiDTO;
using Application.ViewModels.ReviewDTO;
using Application.ViewModels.UserDTO;
using AutoMapper;
using Domain.Entities;
using Domain.Request;
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
            CreateMap<Review, ReviewDTO>().ReverseMap();
            CreateMap<Review, ReviewRequest>().ReverseMap();
            CreateMap<Category, dCreateCategoryDTO>().ReverseMap();
            //CreateMap<Koi, dViewKoiDTO>().ForMember(dest => dest.CategoryName,
            //      opt => opt.MapFrom(src => src.Category.Name)).ReverseMap();
        }
    }
}
