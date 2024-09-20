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
    public class TokenRepo : GenericRepo<Token>,ITokenRepo
    {
        private readonly ApiContext _dbContext;
        public TokenRepo(ApiContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Token?> cFindByConditionAsync(int userId, string type)
        {
            return await _dbSet
             .Where(t => t.UserId == userId && t.TokenValue != null && t.Type == type)
             .FirstOrDefaultAsync();
        }

        public async Task<Token> cGetTokenByUserIdAsync(int userId)
                => await _context.Tokens
                        .FirstOrDefaultAsync(t => t.UserId == userId);

        public async Task<Token> cGetTokenWithUser(string tokenValue, string type)
        {
            return await _dbContext.Tokens
                                .Include(t => t.User)
                                .FirstOrDefaultAsync(t => t.TokenValue == tokenValue && t.Type == type);
        }
    }
}
