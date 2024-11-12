using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepositories
{
    public interface IUserRepo : IGenericRepo<User>
    {
        Task<bool> cCheckEmailAddressExisted(string sEmail);
        Task<User?> cGetByEmailAsync(string sEmail);
        Task<User> cGetUserByConfirmationToken(string token);
        Task<User> cGetUserByEmailAddressAndPasswordHash(string email, string passwordHash);
        Task<IEnumerable<User?>> GetAllUsersAdmin();
        Task<List<Address>> GetAddresses(int id);
        Task<Address?> GetAddressById(int id);

        int GetCount();
    }
}
