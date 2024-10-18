using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.Utils;
using Application.ViewModels;
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
        private readonly IKoiRepo _koiRepo;
        private readonly IMapper _mapper;
        public KoiService(IUnitOfWork unitOfWork, IMapper mapper, IKoiRepo koiRepo)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _koiRepo = koiRepo;
        }
        //private Koi MapToEntityCreate(cCreateKOIDTO CreateProductDTO)
        //{
        //    return _mapper.Map<Koi>(CreateProductDTO);
        //}

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
                Koi newProduct = new Koi
                {
                    Name = cproduct.Namekoi,
                    Description = cproduct.Descriptionkoi,
                    Price = cproduct.Price,
                    Quantity = cproduct.Quantity,
                    Size = cproduct.Size,
                    Dob = cproduct.Dob,
                    CategoryId = cproduct.Categoryid
                };
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

        public async Task<ServiceResponse<PaginationModel<cKOIDTO>>> cGetAllKoisAsync(int page, int pageSize,
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
        public async Task<ServiceResponse<PaginationModel<Koi>>> dGetFilteredKOIsAsync(dFilterKoiDTO filter)
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
        private List<cKOIDTO> MapToDTO(IEnumerable<Koi> kois)
        {
            return kois.Select(koi => MapToDTO(koi)).ToList();
        }
        private cKOIDTO MapToDTO(Koi koi)
        {
            return new cKOIDTO
            {
                Id = koi.Id,
                NameProduct = koi.Name,
                Dob = koi.Dob,
                DescriptionProduct = koi.Description,
                Price = (double)koi.Price, // Chuyển đổi từ decimal sang double
                Quantity = koi.Quantity,
                CategoryId = koi.CategoryId,
                Size = koi.Size,
                ImageUrls = koi.Images.Select(i => i.ImageUrl).ToList() // Lấy danh sách URL ảnh
            };
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