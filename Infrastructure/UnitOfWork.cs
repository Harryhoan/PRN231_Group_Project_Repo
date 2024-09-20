using Application;
using Application.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApiContext _apiContext;

        public UnitOfWork(ApiContext apiContext)
        {
            _apiContext = apiContext;
        }

        public IUserRepo UserRepository => UserRepository;

        public ITokenRepo TokenRepo => TokenRepo;

        public Task<int> SaveChangeAsync()
        {
            return _apiContext.SaveChangesAsync();
        }
    }
}
