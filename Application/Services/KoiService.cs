using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.ViewModels.KoiDTO;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections;
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

        private Koi MapToEntityFilter(cCreateKOIDTO CreateProductDTO)
        {
            return _mapper.Map<Koi>(CreateProductDTO);
        }

        private List<dViewKoiDTO> MapToEnumerableEntityView(List<Koi> kois)
        {
            return _mapper.Map<List<dViewKoiDTO>>(kois);
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
        public async Task<ServiceResponse<PaginationModel<Koi>>> dGetFilteredKOIsAsync(dFilterKoiDTO filter)
        {
            var response = new ServiceResponse<PaginationModel<Koi>>();

            try
            {
                var koiList = await _koiRepo.dGetFilteredKois(filter);
                if (koiList == null)
                {
                    throw new ArgumentNullException(nameof(filter));
                }

                var totalRecords = koiList.Count();

                koiList.Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize);
                var paginationModel = new PaginationModel<Koi>
                {
                    Page = filter.PageNumber,
                    TotalPage = (int)Math.Ceiling(totalRecords / (double)filter.PageSize),
                    TotalRecords = totalRecords,
                    ListData = koiList
                };

                response.Data = paginationModel;
                response.Success = true;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to create product: {ex.Message}";
            }
            return response;
        }
        public async Task<ServiceResponse<PaginationModel<Koi>>> dGetAllKois(int pageNumber,int pageSize)
        {
            var response = new ServiceResponse<PaginationModel<Koi>>();

            try
            {
                var koiList = await _koiRepo.GetAllAsync();
                if (koiList == null)
                {
                    throw new ArgumentNullException(nameof(koiList));
                }

                var totalRecords = koiList.Count();

                koiList.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
                var paginationModel = new PaginationModel<Koi>
                {
                    Page = pageNumber,
                    TotalPage = (int)Math.Ceiling(totalRecords / (double)pageSize),
                    TotalRecords = totalRecords,
                    ListData = koiList
                };

                response.Data = paginationModel;
                response.Success = true;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to create product: {ex.Message}";
            }
            return response;
        }

        public async Task<ServiceResponse<dViewKoiDTO>> dGetKOIById(int id)
        {
            var response = new ServiceResponse<dViewKoiDTO>();

            try
            {
                var koi = await _koiRepo.GetByIdAsync(id);
                if (koi == null)
                {
                    throw new ArgumentNullException(nameof(koi));
                }
                response.Data = _mapper.Map<dViewKoiDTO>(koi);
                response.Success = true;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to get koi: {ex.Message}";
            }
            return response;
        }

    }
}
