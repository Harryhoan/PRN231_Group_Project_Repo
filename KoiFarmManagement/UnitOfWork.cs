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
        private readonly IUserRepo _userRepository;
        private readonly ITokenRepo _tokenRepo;
        public UnitOfWork(ApiContext apiContext, IUserRepo userRepository, ITokenRepo tokenRepo)
        {
            _apiContext = apiContext;
            _userRepository = userRepository;
            _tokenRepo = tokenRepo;
        }

        public IUserRepo UserRepository => _userRepository;

        public ITokenRepo TokenRepo => _tokenRepo;

        public Task<int> SaveChangeAsync()
        {
            return _apiContext.SaveChangesAsync();
        }
    }
}
