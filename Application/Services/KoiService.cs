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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public KoiService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
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

                await _unitOfWork.KoiRepo.cAddKoi(newProduct);
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
        public async Task<ServiceResponse<PaginationModel<dViewKoiDTO>>> dGetFilteredKOIsAsync(dFilterKoiDTO filter)
        {
            var response = new ServiceResponse<PaginationModel<Koi>>();

            try
            {
                var koiList = await _unitOfWork.KoiRepo.dGetFilteredKois(filter);
                if (koiList == null)
                {
                    throw new ArgumentNullException(nameof(filter));
                }

                var totalRecords = koiList.Count();

                koiList.Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize);
                var paginationModel = new PaginationModel<dViewKoiDTO>
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
        public async Task<ServiceResponse<PaginationModel<Koi>>> dGetAllKois(int pageNumber, int pageSize)
        {
            var response = new ServiceResponse<PaginationModel<Koi>>();

            try
            {
                var koiList = await _unitOfWork.KoiRepo.GetAllAsync();
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
                var koi = await _unitOfWork.KoiRepo.dGetKoiWithCategory(id);
                if (koi == null)
                {
                    throw new ArgumentNullException(nameof(koi));
                }
                if (koi.Category == null)
                {
                    throw new ArgumentNullException(nameof(koi.Category));
                }
                response.Data = new dViewKoiDTO
                {
                    CategoryName = koi.Category.Name,
                    CategoryId = koi.Category.Id,
                    Description = koi.Description,
                    Dob = koi.Dob,
                    Price = koi.Price,
                    Quantity = koi.Quantity
                };
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