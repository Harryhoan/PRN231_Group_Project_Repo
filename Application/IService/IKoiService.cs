using Application.ServiceResponse;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IService
{
    public interface IKoiService
    {
        Task<ServiceResponse<int>> cCreateKOIAsync(cCreateKOIDTO cproduct);
        Task<ServiceResponse<PaginationModel<cKOIDTO>>> GetAllKoisAsync(int page, int pageSize, string search, string sort);


    }
}
