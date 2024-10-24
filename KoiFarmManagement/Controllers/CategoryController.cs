﻿
using Application.IService;
using Application.ServiceResponse;
using Application.Services;
using Application.ViewModels.CategoryDTO;
using Application.ViewModels.KoiDTO;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoiFarmManagement.Controllers
{
    [EnableCors("Allow")]
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Staff,Admin,Customer")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [Authorize(Roles = "Staff,Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory(dCreateCategoryDTO category)
        {
            var result = await _categoryService.dCreateCategory(category);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.dGetAllCategory();
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


        [Authorize(Roles = "Staff,Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateCategory(aViewCategory category)
        {
            var result = await _categoryService.aUpdateCategory(category);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


    }
}
