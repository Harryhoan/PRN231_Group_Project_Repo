using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.ViewModels.CategoryDTO;
using Application.ViewModels.KoiDTO;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo _categoryRepo;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepo categoryRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }
        private Category MapToEntityCreate(dCreateCategoryDTO createCategoryDTO)
        {
            return _mapper.Map<Category>(createCategoryDTO);
        }

        private List<dCreateCategoryDTO> MapToCategoryList(List<Category> categories)
        {
            return _mapper.Map<List<dCreateCategoryDTO>>(categories);
        }
        public async Task<ServiceResponse<int>> dCreateCategory(dCreateCategoryDTO createCategoryDTO)
        {
            var response = new ServiceResponse<int>();

            try
            {
                var category = MapToEntityCreate(createCategoryDTO);
                category.Id = 0;

                await _categoryRepo.AddAsync(category);
                response.Data = category.Id;
                response.Success = true;
                response.Message = "Category created successfully";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to create product: {ex.Message}";
            }
            return response;
        }
        public async Task<ServiceResponse<List<dCreateCategoryDTO>>> dGetAllCategory()
        {
            var response = new ServiceResponse<List<dCreateCategoryDTO>>();

            try
            {
                var categories = await _categoryRepo.GetAllAsync();
                if (categories == null)
                {
                    throw new ArgumentNullException(nameof(categories));
                }
                response.Data = MapToCategoryList(categories);
                response.Success = true;
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
