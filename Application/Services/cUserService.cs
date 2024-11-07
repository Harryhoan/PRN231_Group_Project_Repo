using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.Utils;
using Application.ViewModels.UserDTO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class cUserService : cIUserService
    {
        private readonly IUserRepo _cUserRepo;
        private readonly IMapper _mapper;
        public cUserService(IUserRepo userRepo, IMapper mapper)
        {
            _cUserRepo = userRepo;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<PaginationModel<cUserDTO>>> GetAllUsersByAdmin(int page, int pageSize,
             string search, string sort)
        {
            var response = new ServiceResponse<PaginationModel<cUserDTO>>();
            try
            {
                var users = await _cUserRepo.GetAllUsersAdmin();
                if (!string.IsNullOrEmpty(search))
                {
                    users = users
                        .Where(u => u != null && (u.FullName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                                  u.Email.Contains(search, StringComparison.OrdinalIgnoreCase)));
                }
                users = sort.ToLower() switch
                {
                    "name" => users.OrderBy(u => u?.FullName),
                    "email" => users.OrderBy(u => u?.Email),
                    "status" => users.OrderBy(u => u?.AccountLocked),
                    _ => users.OrderBy(u => u?.Id).ToList()
                };
                var userDTOs = _mapper.Map<IEnumerable<cUserDTO>>(users);

                var paginationModel =
                    await cPagination.GetPaginationIENUM(userDTOs, page, pageSize); // Adjust pageSize as needed

                response.Data = paginationModel;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to retrieve admin users: {ex.Message}";
            }
            return response;
        }

        public int GetCount()
        {
            return _cUserRepo.GetCount();
        }
    }
}
