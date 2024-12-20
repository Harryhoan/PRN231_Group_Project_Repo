﻿using Application.IRepositories;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        private Category MapToEntityCreate(dCreateCategoryDTO createCategoryDTO)
        {
            return _mapper.Map<Category>(createCategoryDTO);
        }

        private List<aViewCategory> MapToCategoryList(List<Category> categories)
        {
            return _mapper.Map<List<aViewCategory>>(categories);
        }
        public async Task<ServiceResponse<int>> dCreateCategory(dCreateCategoryDTO createCategoryDTO)
        {
            var response = new ServiceResponse<int>();

            try
            {
                var category = MapToEntityCreate(createCategoryDTO);
                category.Id = 0;

                await _unitOfWork.CategoryRepo.AddAsync(category);
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

        public async Task<ServiceResponse<string>> aUpdateCategory(aViewCategory cat)
        {
            var response = new ServiceResponse<string>();

            try
            {
                var category = await _unitOfWork.CategoryRepo.GetByIdAsync(cat.Id);
                if (category == null)
                {
                    throw new ArgumentNullException(nameof(category));
                }
                await _unitOfWork.CategoryRepo.Update(category);
                response.Success = true;
                response.Message = "Category updated successfully.";
                response.Data = response.Message;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to create product: {ex.Message}";
            }
            return response;
        }
        public async Task<ServiceResponse<List<aViewCategory>>> dGetAllCategory()
        {
            var response = new ServiceResponse<List<aViewCategory>>();

            try
            {
                var categories = await _unitOfWork.CategoryRepo.GetAllAsync();
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