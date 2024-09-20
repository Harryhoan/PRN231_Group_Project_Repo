using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.ViewModels;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class KoiService : IKoiService
    {
        private readonly IKoiRepo _koiRepo;
        private readonly IMapper _mapper;
        public KoiService(IKoiRepo koiRepo, IMapper mapper)
        {
            _koiRepo = koiRepo;
            _mapper = mapper;
        }
        private Koi MapToEntityCreate(cCreateKOIDTO CreateProductDTO)
        {
            return _mapper.Map<Koi>(CreateProductDTO);
        }
        public async Task<ServiceResponse<int>> cCreateKOIAsync(cCreateKOIDTO cproduct)
        {
            var response = new ServiceResponse<int>();

            try
            {
                var newProduct = MapToEntityCreate(cproduct);
                newProduct.Id = 0;

                await _koiRepo.cAddKoi(newProduct);
                response.Data = newProduct.Id;
                response.Success = true;
                response.Message = "Product created successfully";           
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to create product: {ex.Message}";
            }
            return response;
        }
    }
}
