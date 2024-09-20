using Application.ServiceResponse;
using Application.ViewModels.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IService
{
    public interface IAuthenService
    {
        public Task<ServiceResponse<cRegisterDTO>> cRegisterAsync(cRegisterDTO userObject);
        public Task<TokenResponse<string>> cLoginAsync(cLoginUserDTO userObject);
        public Task<ServiceResponse<string>> cResendConfirmationTokenAsync(string email);

    }
}
