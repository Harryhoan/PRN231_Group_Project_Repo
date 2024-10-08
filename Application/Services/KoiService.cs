using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.Utils;
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

        public async Task<ServiceResponse<PaginationModel<cKOIDTO>>> GetAllKoisAsync(int page, int pageSize, 
            string search, string sort)
        {
            var response = new ServiceResponse<PaginationModel<cKOIDTO>>();

            try
            {
                var products = await _koiRepo.GetAllKOI();
                if (!string.IsNullOrEmpty(search))
                {
                    products = products.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
                }

                products = sort.ToLower() switch
                {
                    "name" => products.OrderBy(p => p.Name),
                    "price" => products.OrderBy(p => p.Price),
                    "quantity" => products.OrderBy(p => p.Quantity),
                    "category" => products.OrderBy(p => p.CategoryId),
                    _ => products.OrderBy(p => p.Id)
                };
                var productDTOs = MapToDTO(products); // Map products to ProductDTO

                // Apply pagination
                var paginationModel = await cPagination.GetPaginationIENUM(productDTOs, page, pageSize);

                response.Data = paginationModel;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to retrieve products: {ex.Message}";
            }

            return response;
        }
        private IEnumerable<cKOIDTO> MapToDTO(IEnumerable<Koi> kois)
        {
            return kois.Select(MapToDTO);
        }
        private cKOIDTO MapToDTO(Koi koi)
        {
            var productDTO = _mapper.Map<cKOIDTO>(koi);
            productDTO.ImageUrls = koi.Images?.Select(pi => pi.ImageUrl).ToList();
            return productDTO;
        }
    }
}
