using Application.IRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepo :  GenericRepo<User>, IUserRepo
    {
        private readonly ApiContext _dbContext;

        public UserRepo(ApiContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> cCheckEmailAddressExisted(string sEmail)
        {
            return await _dbContext.Users.AnyAsync(e => e.Email == sEmail);
        }

        public async Task<User?> cGetByEmailAsync(string sEmail) => await _dbSet.FirstOrDefaultAsync(u => u.Email == sEmail);

        public async Task<User> cGetUserByConfirmationToken(string token)
        {
            var user = await _dbContext.Tokens
                .Where(t => t.TokenValue == token && t.Type == "confirmation")
                .Select(t => t.User)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<User> cGetUserByEmailAddressAndPasswordHash(string email, string passwordHash)
        {
            var user = await _dbContext.Users.Include(u => u.Tokens)
                .FirstOrDefaultAsync(record => record.Email == email && record.Password == passwordHash);          
            return user;
        }
    }
}
