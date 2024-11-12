using Application.ServiceResponse;
using Application.ViewModels.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.IService
{
    public interface cIUserService
    {
        Task<ServiceResponse<PaginationModel<cUserDTO>>> GetAllUsersByAdmin(int page, int pageSize, string search, string sort);
        int GetCount();
        Task<ServiceResponse<ProfileDTO?>> GetUserByResponse(ClaimsPrincipal claims);
        Task<ServiceResponse<ProfileDTO?>> UpdateProfile(ClaimsPrincipal claims, ProfileDTO profileDTO);
        Task<ServiceResponse<List<AddressDTO>>> GetAddressByUser(ClaimsPrincipal claims);
        Task<ServiceResponse<AddressDTO>> UpdateAddress(ClaimsPrincipal claims, AddressDTO addressDTO);
        Task<ServiceResponse<AddressDTO?>> CreateAddress(ClaimsPrincipal claims, AddressDTO addressDTO);
        Task<ServiceResponse<string>> DeleteAdress(ClaimsPrincipal claims, int id);
    }
}
