using Application.ViewModels;
using Application.ViewModels.CategoryDTO;
using Application.ViewModels.KoiDTO;
using Application.ViewModels.OrderDetailDTO;
using Application.ViewModels.OrderDTO;
using Application.ViewModels.ReviewDTO;
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
            CreateMap<Koi, cGetKoiByIdAdmin>().ReverseMap();
            CreateMap<User, cLoginUserDTO>().ReverseMap();
            CreateMap<Koi, cCreateKOIDTO>().ReverseMap();
            CreateMap<User, cUserDTO>().ReverseMap();
            CreateMap<Koi, cKOIDTO>().ReverseMap();
            CreateMap<Review, ReviewDTO>().ReverseMap();
            CreateMap<Category, dCreateCategoryDTO>().ReverseMap();
            CreateMap<Koi, cUpdateProductDTO>().ReverseMap();
CreateMap<OrderDetail, aCreateOrderDetailDTO>().ReverseMap();
			CreateMap<aOrderDTO, Order>()
			.ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails));
			CreateMap<aViewOrderDetailDTO, OrderDetail>();
			CreateMap<Order, aOrderDTO>()
				.ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails));
			CreateMap<OrderDetail, aViewOrderDetailDTO>();
            CreateMap<aEditReviewDTO, Review>().ReverseMap();
            CreateMap<aViewCategory, Category>().ReverseMap();
            CreateMap<Review, ReviewRequestDTO>().ReverseMap();
            //CreateMap<Koi, dViewKoiDTO>().ForMember(dest => dest.CategoryName,
            //      opt => opt.MapFrom(src => src.Category.Name)).ReverseMap();
        }
    }	
}
