using Application.ServiceResponse;
using Application.ViewModels.CategoryDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IService
{
    public interface ICategoryService
    {
        Task<ServiceResponse<int>> dCreateCategory(dCreateCategoryDTO createCategoryDTO);
        Task<ServiceResponse<string>> aUpdateCategory(aViewCategory cat);

    }
}
