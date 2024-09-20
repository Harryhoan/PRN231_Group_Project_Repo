using Application.IService;
using Application.ServiceResponse;
using Application.ViewModels.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthenService : IAuthenService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthenService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<ServiceResponse<cRegisterDTO>> RegisterAsync(cRegisterDTO userObject)
        {
            throw new NotImplementedException();
        }
    }
}
