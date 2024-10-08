using Application.ServiceResponse;
using Application.ViewModels.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IService
{
    public interface cIUserService
    {
        Task<ServiceResponse<PaginationModel<cUserDTO>>> GetAllUsersByAdmin(int page, int pageSize, string search, string sort);

    }
}
