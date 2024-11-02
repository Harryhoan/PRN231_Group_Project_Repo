using Application.ServiceResponse;
using Application.ViewModels;
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
        Task<ServiceResponse<PaginationModel<cKOIDTO>>> cGetAllKoisAsync(int page, int pageSize, string search, string sort);

        Task<ServiceResponse<PaginationModel<dViewKoiDTO>>> dGetFilteredKOIsAsync(dFilterKoiDTO filter);
        Task<ServiceResponse<PaginationModel<Koi>>> dGetAllKois(int pageNumber, int pageSize);
        Task<ServiceResponse<dViewKoiDTO>> dGetKOIById(int id);
        Task<ServiceResponse<cGetKoiByIdAdmin>> cGetKoibyAdmin(int id);
        Task<ServiceResponse<string>> cUpdateProductAsync(cUpdateProductDTO cproduct);
        Task<ServiceResponse<string>> cDeleteProductAsync(int id);

    }
}
