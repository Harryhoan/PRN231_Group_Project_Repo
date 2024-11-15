using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.Utils;
using Application.ViewModels.KoiDTO;
using Application.ViewModels.UserDTO;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class cUserService : cIUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepo _cUserRepo;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IMapper _mapper;
        public cUserService(IUserRepo userRepo, IMapper mapper, IOrderDetailService orderDetailService, IUnitOfWork unitOfWork)
        {
            _cUserRepo = userRepo;
            _mapper = mapper;
            _orderDetailService = orderDetailService;
            _unitOfWork = unitOfWork;
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
        public async Task<ServiceResponse<List<AddressDTO>>> GetAddressByUser(ClaimsPrincipal claims)
        {
            var response = new ServiceResponse<List<AddressDTO>>();
            try
            {
                var user = await _orderDetailService.aGetUserByTokenAsync(claims);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Failed to retrieve user ";
                    return response;

                }
                var addresses = await _cUserRepo.GetAddresses(user.Id);
                if (addresses.Count == 0) {
                    response.Success = true;
                    response.Message = "You dont have any address";
                    return response;

                }
                var addressDTO = _mapper.Map<List<AddressDTO>>(addresses);
                response.Data = addressDTO;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to retrieve user addresses: {ex.Message}";
            }
            return response;
        }
        public async Task<ServiceResponse<AddressDTO?>> CreateAddress(ClaimsPrincipal claims, AddressDTO addressDTO)
        {
            var response = new ServiceResponse<AddressDTO>();
            try
            {
                var user = await _orderDetailService.aGetUserByTokenAsync(claims);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Failed to retrieve user ";
                    return response;

                }

                Address newAddress = new Address
                {
                    District = addressDTO.District,
                    Province = addressDTO.Province,
                    Ward = addressDTO.Ward,
                    Street = addressDTO.Street,
                    User = user,
                    UserId = user.Id
                };
                newAddress.Id = 0;
                await _unitOfWork.AddressRepo.AddAsync(newAddress);
                response.Message = "Adress add Successfully";
                response.Data = addressDTO;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to add address: {ex.Message}";
            }
            return response;
        }
        public async Task<ServiceResponse<string>> DeleteAdress(ClaimsPrincipal claims, int id)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var user = await _orderDetailService.aGetUserByTokenAsync(claims);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Failed to retrieve user";
                    return response;

                }
                var address = await _cUserRepo.GetAddressById(id);
                if (address == null)
                {
                    response.Success = false;
                    response.Message = "No address Id found ";
                    return response;
                }
                if (address.UserId != user.Id) {
                    response.Success = false;
                    response.Message = "You dont have permision to delete this address";
                    return response;
                }
                await _unitOfWork.AddressRepo.cDeleteTokenAsync(address);
                response.Data = "Adress remove Successfully";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to remove address: {ex.Message}";
            }
            return response;
        }

        public async Task<ServiceResponse<AddressDTO>> UpdateAddress(ClaimsPrincipal claims, AddressDTO addressDTO)
        {
            var response = new ServiceResponse<AddressDTO>();
            try
            {
                var user = await _orderDetailService.aGetUserByTokenAsync(claims);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Failed to retrieve user ";
                    return response;

                }

                var address = await _cUserRepo.GetAddressById(addressDTO.Id);
                if (address == null) 
                {
                    response.Success = false;
                    response.Message = "Address Id not found";
                    return response;
                }
                if (address.UserId != user.Id)
                {
                    response.Success = false;
                    response.Message = "You dont have permision to update this address";
                    return response;
                }
                // Update the product in the repository
                MapAddress(addressDTO, address);

                await _unitOfWork.AddressRepo.Update(address);
                response.Message = "Adress Update Successfully";
                response.Data = addressDTO;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to retrieve user profile: {ex.Message}";
            }
            return response;
        }

        public async Task<ServiceResponse<ProfileDTO>> GetUserByResponse(ClaimsPrincipal claims)
        {
            var response = new ServiceResponse<ProfileDTO>();
            try
            {
                var user = await _orderDetailService.aGetUserByTokenAsync(claims);
                var userDTOs = _mapper.Map<ProfileDTO>(user);
                response.Data = userDTOs;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to retrieve user profile: {ex.Message}";
            }
            return response;
        }
        private void MapUser(ProfileDTO profile, User user)
        {
            user.FullName = profile.FullName;
            user.Email = profile.Email;
            user.TelephoneNumber = profile.TelephoneNumber;
        }
        private void MapAddress(AddressDTO addressDTO, Address address)
        {
            address.Id = addressDTO.Id;
            address.Ward = addressDTO.Ward;
            address.District = addressDTO.District;
            address.Province = addressDTO.Province;
            address.Street = addressDTO.Street;
        }

        public async Task<ServiceResponse<ProfileDTO?>> UpdateProfile(ClaimsPrincipal claims, ProfileDTO profileDTO)
        {
            var response = new ServiceResponse<ProfileDTO>();
            try
            {
                var user = await _orderDetailService.aGetUserByTokenAsync(claims);
                if (user == null)
                {
                    response.Message = "User not found";
                    response.Success = false;
                    return response;
                }
                if (profileDTO.Email != user.Email)
                {
                        response.Message = "Email cant be change";
                        response.Success = false;
                        return response;
                }
                // Update the product in the repository
                MapUser(profileDTO, user);

                await _cUserRepo.Update(user);
                response.Message = "Profile Update Successfully";
                response.Data = profileDTO;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to retrieve user profile: {ex.Message}";
            }
            return response;
        }

        public int GetCount()
        {
            return _cUserRepo.GetCount();
        }
    }
}
