using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepositories
{
    public interface ITokenRepo : IGenericRepo<Token>
    {
        public Task<Token?> cFindByConditionAsync(int userId, string type);
        public Task<Token> cGetTokenWithUser(string tokenValue, string type); 
        public Task<Token> cGetTokenByUserIdAsync(int userId);
    }
}
