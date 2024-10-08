using Application.ServiceResponse;
using Application.ViewModels.KoiDTO;
using Domain.Entities;
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

        Task<ServiceResponse<PaginationModel<Koi>>> dGetFilteredKOIsAsync(dFilterKoiDTO filter);
        Task<ServiceResponse<PaginationModel<Koi>>> dGetAllKois(int pageNumber, int pageSize);
        Task<ServiceResponse<dViewKoiDTO>> dGetKOIById(int id);

    }
}
